using Combat.Actor;

namespace Combat {
    public static class CombatUtils {
        public static int SelfUid => GameMgr.Instance.GetSystem<ICombatSystem>()?.SelfUid ?? -1;
        
        public static bool IsSameCamp(int aUid, int bUid) {
            return GameMgr.Instance.GetSystem<IActorSystem>()?.IsSameCamp(aUid, bUid) ?? false;
        }
        
        public static bool IsSameCamp(int uid) {
            return GameMgr.Instance.GetSystem<IActorSystem>()?.IsSameCamp(SelfUid, uid) ?? false;
        }
    }
}