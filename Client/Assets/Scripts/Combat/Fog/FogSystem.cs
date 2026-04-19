using System;
using UnityEngine;

namespace Combat.Fog {
    public class FogSystem : IFogSystem {
        private readonly Vision vision = new Vision();
        private Material fog;
        private Texture2D fogTexture;
        private Color32[] fogPixels;
        private static readonly int FogTexId = Shader.PropertyToID("_FogTex");

        public void Init() {
            fog = GameObject.Find("Fog")?.GetComponent<SpriteRenderer>()?.material;
        }

        public void Start() {
            vision.Init();
        }

        public void Update() {
            UpdateFog();
        }
        
        public Action AddVision(Vector3F position, FloatF radius) {
            return vision.AddVision(position, radius);
        }

        private void UpdateFog() {
            if (fog == null) {
                return;
            }

            if (fogTexture == null) {
                fogTexture = new Texture2D(FogConfig.VisionCellCount, FogConfig.VisionCellCount, TextureFormat.RGBA32, false);
                fogTexture.filterMode = FilterMode.Bilinear;
                fogTexture.wrapMode = TextureWrapMode.Clamp;
                fogPixels = new Color32[FogConfig.VisionCellCount * FogConfig.VisionCellCount];
                fog.SetTexture(FogTexId, fogTexture);
            }
            
            for (int x = 0; x < FogConfig.VisionCellCount; x++) {
                for (int y = 0; y < FogConfig.VisionCellCount; y++) {
                    byte mask = vision.IsVisible(x, y) ? (byte)0 : (byte)255;
                    if (x < FogConfig.VisionCellCount / 2 && y < FogConfig.VisionCellCount / 2) {
                        mask = 0;
                    }
                    fogPixels[x * FogConfig.VisionCellCount + y] = new Color32(mask, mask, mask, 255);
                }
            }
            
            fogTexture.SetPixels32(fogPixels);
            fogTexture.Apply(false, false);
        }
    }
}
