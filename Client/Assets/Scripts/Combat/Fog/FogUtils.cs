using System;
using Combat.Fog;

public static class FogUtils {
    public static Action AddVision(Vector3F position, FloatF radius) {
        return GameMgr.Instance.GetSystem<IFogSystem>()?.AddVision(position, radius);
    }
}
