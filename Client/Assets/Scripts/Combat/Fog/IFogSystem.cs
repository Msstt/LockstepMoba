using System;
using UnityEngine;

namespace Combat.Fog {
    public interface IFogSystem : ISystem, IInitSystem, IStartSystem, IUpdateSystem {
        public Action AddVision(Vector3F position, FloatF radius);
        public Texture2D FogTexture { get; }
    }
}