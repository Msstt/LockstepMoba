using Combat;
using Combat.Actor;

public static class ActorUtils {
    public static bool IsSameCamp(int aUid, int bUid) {
        return GameMgr.Instance.GetSystem<IActorSystem>()?.IsSameCamp(aUid, bUid) ?? false;
    }
    
    public static bool IsSameCamp(int uid) {
        return GameMgr.Instance.GetSystem<IActorSystem>()?.IsSameCamp(CombatUtils.SelfUid, uid) ?? false;
    }
    
    public static Actor GetActor(int uid) {
        return GameMgr.Instance.GetSystem<IActorSystem>()?.GetActor(uid);
    }
    
    public static Actor GetActor() {
        return GameMgr.Instance.GetSystem<IActorSystem>()?.GetActor(CombatUtils.SelfUid);
    }
}
