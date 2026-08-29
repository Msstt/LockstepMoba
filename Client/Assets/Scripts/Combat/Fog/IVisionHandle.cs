using System;

namespace Combat.Fog {
    public interface IVisionHandle : IDisposable {
        public void UpdatePos(Vector3F position);
    }
}