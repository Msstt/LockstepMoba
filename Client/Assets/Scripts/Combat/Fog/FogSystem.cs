using System;
using Framework;
using UnityEngine;

namespace Combat.Fog {
    public class FogSystem : IFogSystem {
        private const int UpscaleFactor = 4;
        private const float FadeSpeed = 5f;
        private readonly int SourceSize = FogConfig.VisionCellCount;
        private readonly int UpscaledSize = FogConfig.VisionCellCount * UpscaleFactor;

        private Vision[] vision;
        private Texture2D fogTexture;
        private Color32[] fogPixels;

        private RenderTexture rtUpscaled;
        private RenderTexture rtBlurH;
        private RenderTexture rtBlurFinal;
        private RenderTexture rtDisplay;
        private RenderTexture rtPrev;

        private Material upscaleMaterial;
        private Material blurMaterial;
        private Material fadeMaterial;
        private const float BlurOffset = 2.0f;

        public Texture2D FogTexture => fogTexture;
        
        private VisionBlockerMap blockerMap;
        private VisionType lastVisionType = VisionType.None;

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
            lastVisionType = CurVisionType;
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

            rtDisplay = new RenderTexture(desc);
            rtDisplay.filterMode = FilterMode.Bilinear;
            rtDisplay.wrapMode = TextureWrapMode.Clamp;
            rtDisplay.Create();

            rtPrev = new RenderTexture(desc);
            rtPrev.filterMode = FilterMode.Bilinear;
            rtPrev.wrapMode = TextureWrapMode.Clamp;
            rtPrev.Create();

            Shader.SetGlobalTexture("_FogTex", rtDisplay);
            Shader.SetGlobalFloat("_FogSourceSize", SourceSize);
        }

        private void InitMaterials() {
            Shader upscaleShader = Shader.Find("Hidden/FogOfWarUpscale");
            Shader blurShader = Shader.Find("Hidden/FogOfWarBlur");
            Shader fadeShader = Shader.Find("Hidden/FogOfWarFade");

            upscaleMaterial = new Material(upscaleShader);
            blurMaterial = new Material(blurShader);
            blurMaterial.SetFloat(Shader.PropertyToID("_FogBlurOffset"), BlurOffset);
            fadeMaterial = new Material(fadeShader);
        }

        private void ProcessFog() {
            Graphics.Blit(fogTexture, rtUpscaled, upscaleMaterial);
            Graphics.Blit(rtUpscaled, rtBlurH, blurMaterial, 0);
            Graphics.Blit(rtBlurH, rtBlurFinal, blurMaterial, 1);

            float fadeStep = Mathf.Clamp01(Time.deltaTime * FadeSpeed);
            // 切换视野域时，直接写入纹理
            if (lastVisionType != CurVisionType) {
                fadeStep = 1;
            }
            fadeMaterial.SetFloat("_FadeStep", fadeStep);
            fadeMaterial.SetTexture("_PrevTex", rtPrev);
            Graphics.Blit(rtBlurFinal, rtDisplay, fadeMaterial);

            Graphics.Blit(rtDisplay, rtPrev);
        }

        private VisionType CurVisionType {
            get {
                // 暂时先这样
                // 如果被设置过受限视野，那一定是中 debuff 了，需要切换到受限视野的
                if (vision[(int)VisionType.Limit].VisionCount > 0) {
                    return VisionType.Limit;
                }
                return VisionType.Self;
            }
        }
        
        private bool IsVisible(int x, int y) {
            return vision[(int)CurVisionType].IsVisible(x, y);
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
            ReleaseRT(ref rtUpscaled);
            ReleaseRT(ref rtBlurH);
            ReleaseRT(ref rtBlurFinal);
            ReleaseRT(ref rtDisplay);
            ReleaseRT(ref rtPrev);

            if (fogTexture != null) {
                UnityEngine.Object.Destroy(fogTexture);
                fogTexture = null;
            }

            DestroyMaterial(ref upscaleMaterial);
            DestroyMaterial(ref blurMaterial);
            DestroyMaterial(ref fadeMaterial);
        }

        private static void ReleaseRT(ref RenderTexture rt) {
            if (rt != null) {
                rt.Release();
                rt = null;
            }
        }

        private static void DestroyMaterial(ref Material mat) {
            if (mat != null) {
                UnityEngine.Object.Destroy(mat);
                mat = null;
            }
        }
    }
}