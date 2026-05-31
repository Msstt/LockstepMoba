using System;
using UnityEngine;

namespace Combat.Fog {
    public class FogSystem : IFogSystem {
        private readonly Vision vision = new Vision();
        private Texture2D fogTexture;
        private Color32[] fogPixels;

        public Texture2D FogTexture => fogTexture;

        public void Init() {
            fogTexture = new Texture2D(FogConfig.VisionCellCount, FogConfig.VisionCellCount, TextureFormat.RGBA32, false);
            fogTexture.filterMode = FilterMode.Bilinear;
            fogTexture.wrapMode = TextureWrapMode.Clamp;
            fogPixels = new Color32[FogConfig.VisionCellCount * FogConfig.VisionCellCount];
            
            Material material = Resources.Load<Material>("Material/FogOfWar");
            material.SetTexture(Shader.PropertyToID("_FogTex"), fogTexture);
            
            vision.Init();
        }

        public void Start() {
        }

        public void Update() {
            UpdateFog();
        }
        
        public Action AddVision(Vector3F position, FloatF radius) {
            return vision.AddVision(position, radius);
        }

        private void UpdateFog() {
            for (int x = 0; x < FogConfig.VisionCellCount; x++) {
                for (int y = 0; y < FogConfig.VisionCellCount; y++) {
                    byte mask = vision.IsVisible(x, y) ? (byte)255 : (byte)0;
                    fogPixels[y * FogConfig.VisionCellCount + x] = new Color32(mask, mask, mask, 255);
                }
            }
            
            fogTexture.SetPixels32(fogPixels);
            fogTexture.Apply(false, false);
        }

        public void Quit() {
            if (fogTexture != null) {
                UnityEngine.Object.Destroy(fogTexture);
                fogTexture = null;
            }
        }
    }
}
