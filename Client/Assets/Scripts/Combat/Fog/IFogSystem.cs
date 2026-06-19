using System;
using UnityEngine;

namespace Combat.Fog {
    public interface IFogSystem : ISystem, IInitSystem, IStartSystem, IUpdateSystem, IQuitSystem {
        public Action AddVision(VisionType type, Vector3F position, FloatF radius);
        public Texture2D FogTexture { get; }

        public bool IsVisible(Vector3F position);
    }
}