using System;
using System.Collections.Generic;
using Framework;

namespace Navmesh {
    public interface INavmesh : IInitSystem, IFrameUpdateSystem {
        public NavmeshMapInfo MapInfo { get; }

        public void FindPath(FloatF radius, Vector3F start, Vector3F end, Action<List<Vector3F>> callback, bool force);

        public bool IsReachable(FloatF radius, Vector3F point);
        public Vector3F RaycastInSurface(FloatF radius, Vector3F start, Vector3F end);

        public void RegisterUnit(int id, int type, Vector3F pos, SafeEvent<Vector3F> onPosChange);
        public void UnRegisterUnit(int id, SafeEvent<Vector3F> onPosChange);
        public void RaycastInCircle(int typeBitSet, Vector3F center, FloatF radius, List<int> results);
        public void RaycastInRect(int typeBitSet, Vector3F center, Vector3F direction, FloatF length, FloatF width, List<int> results);
        
        public float GetHeight(float x, float y);
    }
}
