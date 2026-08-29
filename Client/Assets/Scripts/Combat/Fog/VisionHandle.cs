// 更新位置时如果所在的视野格没有更新，则不需要更新视野

using System;
using Framework;

namespace Combat.Fog {
    public class VisionHandle : IVisionHandle {
        private Vision vision;
        private Vector3F lastPos;
        private FloatF radius;

        private ReleaseToken releaseToken;
        
        public VisionHandle(Vision vision, Vector3F position, FloatF rowRadius) {
            this.vision = vision;
            lastPos = position;
            radius = rowRadius;
            releaseToken = vision.AddVision(position, rowRadius);
        }
        
        public void UpdatePos(Vector3F position) {
            if (releaseToken == null) {
                return;
            }
            
            if (vision.GetCellIndex(lastPos) == vision.GetCellIndex(position)) {
                return;
            }

            lastPos = position;
            releaseToken.Release();
            releaseToken = vision.AddVision(position, radius);
        }

        public void Dispose() {
            releaseToken?.Release();
            releaseToken = null;
        }
    }
}