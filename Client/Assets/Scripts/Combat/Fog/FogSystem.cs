using System;
using Framework;
using UnityEngine;

namespace Combat.Fog {
    public class FogSystem : IFogSystem {
        private const int UpscaleFactor = 4;
        private readonly int SourceSize = FogConfig.VisionCellCount;
        private readonly int UpscaledSize = FogConfig.VisionCellCount * UpscaleFactor;

        private Vision[] vision;
        private Texture2D fogTexture;
        private Color32[] fogPixels;

        private RenderTexture rtUpscaled;
        private RenderTexture rtBlurH;
        private RenderTexture rtBlurFinal;

        private Material upscaleMaterial;
        private Material blurMaterial;
        private const float BlurOffset = 2.0f;

        public Texture2D FogTexture => fogTexture;
        
        private VisionBlockerMap blockerMap;

        public void Init() {
            InitFogTexture();
            InitRenderTextures();
            InitMaterials();
            
            try {
                JsonHelper.LoadFromString(NavmeshUtils.Config.visionBlockerMap.text, out blockerMap);
            } catch (Exception e) {
                Log.Error("视野遮罩图解析失败: " + e);
            }
            Shader.SetGlobalVector("_FogStart", blockerMap.Start.ToVector3());
            Shader.SetGlobalVector("_FogCellSize", blockerMap.CellSize.ToVector3());

            int typeCount = Enum.GetNames(typeof(VisionType)).Length;
            vision = new Vision[typeCount];
            vision[(int)VisionType.Global] = new OrVision(blockerMap);
            vision[(int)VisionType.Self] = new OrVision(blockerMap);
            vision[(int)VisionType.Limit] = new AndVision(blockerMap);
        }

        public void Start() {
        }

        public void Update() {
            UpdateFog();
            ProcessFog();
        }
        
        public Action AddVision(VisionType type, Vector3F position, FloatF radius) {
            return vision[(int)type].AddVision(position, radius);
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

            Shader.SetGlobalTexture("_FogTex", rtBlurFinal);
            Shader.SetGlobalFloat("_FogSourceSize", SourceSize);
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
        
        private bool IsVisible(int x, int y) {
            // 暂时先这样
            // 如果被设置过受限视野，那一定是中 debuff 了，需要切换到受限视野的
            if (vision[(int)VisionType.Limit].VisionCount > 0) {
                return vision[(int)VisionType.Limit].IsVisible(x, y);
            }
            return vision[(int)VisionType.Self].IsVisible(x, y);
        }

        private void UpdateFog() {
            for (int x = 0; x < SourceSize; x++) {
                for (int y = 0; y < SourceSize; y++) {
                    byte mask = IsVisible(x, y) ? (byte)255 : (byte)0;
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