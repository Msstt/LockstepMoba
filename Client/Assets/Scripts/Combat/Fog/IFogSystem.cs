using System;

namespace Combat.Fog {
    public interface IFogSystem : ISystem, IInitSystem, IStartSystem, IUpdateSystem {
        public Action AddVision(Vector3F position, FloatF radius);
    }
}