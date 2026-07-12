using System;
using Combat.Actor;
using Combat.Area;
using Combat.Fog;

public static class FogUtils {
    public static Action AddVision(VisionType type, Vector3F position, FloatF radius) {
        return GameMgr.Instance.GetSystem<IFogSystem>()?.AddVision(type, position, radius);
    }
    
    public static bool IsVisible(Vector3F position) {
        return GameMgr.Instance.GetSystem<IFogSystem>()?.IsVisible(position) ?? false;
    }

    public static bool IsVisible(Actor actor) {
        return (actor.Stats.Invisibility == 0 || ActorUtils.IsSameCamp(actor.Uid)) && IsVisible(actor.Pos);
    }
    
    public static bool IsVisible(Area area) {
        // TODO 检测区域
        return IsVisible(area.Position);
    }
}
