using Combat.Actor;

namespace Combat {
    public static class CombatUtils {
        public static int SelfUid => GameMgr.Instance.GetSystem<ICombatSystem>()?.SelfUid ?? -1;
        
        public static int GetChampionId(Uid uid) => GameMgr.Instance.GetSystem<ICombatSystem>()?.GetChampionId(uid) ?? -1;
        public static CampType GetCamp(Uid uid) => GameMgr.Instance.GetSystem<ICombatSystem>()?.GetCamp(uid) ?? CampType.UnKnown;
    }
}