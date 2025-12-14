using Combat;
using Network;

public class BattleMsgDispatcher : MsgDispatcher {
    public static void Register() {
        dispatcher.RegisterHandler<battle_start_s2c>(MessageDef.battle_start_s2c, battle_start_s2c);
    }
    
    private static void battle_start_s2c(battle_start_s2c msg) {
        if (CombatMgr.Instance.IsRunning) {
            return;
        }
        CombatMgr.Instance.Start(msg);
        LockStep.Instance.Start();
    }
}