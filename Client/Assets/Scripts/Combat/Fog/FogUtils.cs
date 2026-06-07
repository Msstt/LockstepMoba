using System;
using Combat.Fog;

public static class FogUtils {
    public static Action AddVision(VisionType type, Vector3F position, FloatF radius) {
        return GameMgr.Instance.GetSystem<IFogSystem>()?.AddVision(type, position, radius);
    }
}
