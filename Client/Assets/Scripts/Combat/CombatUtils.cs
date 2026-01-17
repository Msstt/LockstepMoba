using Combat.Actor;

namespace Combat {
    public static class CombatUtils {
        public static int SelfUid => GameMgr.Instance.GetSystem<ICombatSystem>()?.SelfUid ?? -1;
    }
}