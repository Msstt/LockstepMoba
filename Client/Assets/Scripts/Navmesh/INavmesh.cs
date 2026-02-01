using System;
using System.Collections.Generic;

namespace Navmesh {
    public interface INavmesh : IInitSystem, IFrameUpdateSystem {
        public NavmeshMapInfo MapInfo { get; }

        public void FindPath(FloatF radius, Vector3F start, Vector3F end, Action<List<Vector3F>> callback, bool force);

        public bool IsReachable(FloatF radius, Vector3F point);
        public Vector3F RaycastInSurface(FloatF radius, Vector3F start, Vector3F end);
    }
}