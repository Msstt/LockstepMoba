using Combat;
using Network;

public class BattleMsgDispatcher : MsgDispatcher {
    public static void Register() {
        Register<battle_start_s2c>(MessageDef.battle_start_s2c, battle_start_s2c);
    }
    
    private static void battle_start_s2c(battle_start_s2c msg) {
        if (GameMgr.Instance.IsRunning) {
            return;
        }

        GameMgr.Instance.GetSystem<ICombatSystem>().Init(msg);
        GameMgr.Instance.Start();
    }
}