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

    public static Actor GetActor() => GetActor(CombatUtils.SelfUid);
    
    public static T GetCom<T>(int uid) where T : Com {
        T com = GameMgr.Instance.GetSystem<IActorSystem>()?.GetPersistentCom<T>(uid);
        if (com != null) {
            return com;
        }
        return GetActor(uid)?.GetComponent<T>();
    }

    public static T GetCom<T>() where T : Com => GetCom<T>(CombatUtils.SelfUid);
    
    public static T GetPersistentCom<T>(int uid) where T : Com {
        return GameMgr.Instance.GetSystem<IActorSystem>()?.GetPersistentCom<T>(uid);
    }
    
    public static T GetPersistentCom<T>() where T : Com => GetPersistentCom<T>(CombatUtils.SelfUid);
}
