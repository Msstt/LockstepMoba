using Battle;
using Network;

public class BattleMsgDispatcher : MsgDispatcher {
    public static void Register() {
        dispatcher.RegisterMsgHandler<select_champion_c2s>(MessageDef.select_champion_c2s, select_champion_c2s);
    }
    
    [Message(MessageDef.select_champion_c2s)]
    private static void select_champion_c2s(Uid uid, select_champion_c2s msg) {
        Match.Instance.SetChampion(uid, msg.ChampionId);
    }
}