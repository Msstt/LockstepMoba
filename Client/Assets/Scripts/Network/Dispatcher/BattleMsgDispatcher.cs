using Battle;
using Network;

public static class BattleMsgDispatcher {
    [Message(MessageDef.battle_start_s2c)]
    public static void battle_start_s2c(battle_start_s2c msg) {
        BattleMgr.Instance.Start(msg);
        LockStep.Instance.Start();
    }
}