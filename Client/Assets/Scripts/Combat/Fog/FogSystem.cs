using System;
using UnityEngine;

namespace Combat.Fog {
    public class FogSystem : IFogSystem {
        private const int UpscaleFactor = 4;
        private readonly int SourceSize = FogConfig.VisionCellCount;
        private readonly int UpscaledSize = FogConfig.VisionCellCount * UpscaleFactor;

        private readonly Vision vision = new Vision();
        private Texture2D fogTexture;
        private Color32[] fogPixels;

        private RenderTexture rtUpscaled;
        private RenderTexture rtBlurH;
        private RenderTexture rtBlurFinal;

        private Material upscaleMaterial;
        private Material blurMaterial;
        private const float BlurOffset = 2.0f;

        public Texture2D FogTexture => fogTexture;

        public void Init() {
            InitFogTexture();
            InitRenderTextures();
            InitMaterials();

            vision.Init();
        }

        public void Start() {
        }

        public void Update() {
            UpdateFog();
            ProcessFog();
        }
        
        public Action AddVision(Vector3F position, FloatF radius) {
            return vision.AddVision(position, radius);
        }
        
        private void InitFogTexture() {
            fogTexture = new Texture2D(SourceSize, SourceSize, TextureFormat.RGBA32, false);
            fogTexture.filterMode = FilterMode.Point;
            fogTexture.wrapMode = TextureWrapMode.Clamp;
            fogPixels = new Color32[SourceSize * SourceSize];
        }

        private void InitRenderTextures() {
            RenderTextureDescriptor desc = new RenderTextureDescriptor(UpscaledSize, UpscaledSize, RenderTextureFormat.ARGB32, 0);

            rtUpscaled = new RenderTexture(desc);
            rtUpscaled.filterMode = FilterMode.Bilinear;
            rtUpscaled.wrapMode = TextureWrapMode.Clamp;
            rtUpscaled.Create();

            rtBlurH = new RenderTexture(desc);
            rtBlurH.filterMode = FilterMode.Bilinear;
            rtBlurH.wrapMode = TextureWrapMode.Clamp;
            rtBlurH.Create();

            rtBlurFinal = new RenderTexture(desc);
            rtBlurFinal.filterMode = FilterMode.Bilinear;
            rtBlurFinal.wrapMode = TextureWrapMode.Clamp;
            rtBlurFinal.Create();

            Material fogMaterial = Resources.Load<Material>("Material/FogOfWar");
            fogMaterial.SetTexture(Shader.PropertyToID("_FogTex"), rtBlurFinal);
            fogMaterial.SetFloat(Shader.PropertyToID("_FogSourceSize"), SourceSize);
        }

        private void InitMaterials() {
            Shader upscaleShader = Shader.Find("Hidden/FogOfWarUpscale");
            Shader blurShader = Shader.Find("Hidden/FogOfWarBlur");

            upscaleMaterial = new Material(upscaleShader);
            blurMaterial = new Material(blurShader);
            blurMaterial.SetFloat(Shader.PropertyToID("_FogBlurOffset"), BlurOffset);
        }

        private void ProcessFog() {
            Graphics.Blit(fogTexture, rtUpscaled, upscaleMaterial);
            Graphics.Blit(rtUpscaled, rtBlurH, blurMaterial, 0);
            Graphics.Blit(rtBlurH, rtBlurFinal, blurMaterial, 1);
        }

        private void UpdateFog() {
            for (int x = 0; x < SourceSize; x++) {
                for (int y = 0; y < SourceSize; y++) {
                    byte mask = vision.IsVisible(x, y) ? (byte)255 : (byte)0;
                    fogPixels[y * SourceSize + x] = new Color32(mask, mask, mask, 255);
                }
            }

            fogTexture.SetPixels32(fogPixels);
            fogTexture.Apply(false, false);
        }

        public void Quit() {
            if (rtUpscaled != null) {
                rtUpscaled.Release();
                rtUpscaled = null;
            }

            if (rtBlurH != null) {
                rtBlurH.Release();
                rtBlurH = null;
            }

            if (rtBlurFinal != null) {
                rtBlurFinal.Release();
                rtBlurFinal = null;
            }

            if (fogTexture != null) {
                UnityEngine.Object.Destroy(fogTexture);
                fogTexture = null;
            }

            if (upscaleMaterial != null) {
                UnityEngine.Object.Destroy(upscaleMaterial);
                upscaleMaterial = null;
            }
            
            if (blurMaterial != null) { 
                UnityEngine.Object.Destroy(blurMaterial); 
                blurMaterial = null;
            }
        }
    }
}