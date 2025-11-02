using Framework;
using Network;

namespace Battle {
    public class Match : Singleton<Match> {
        public void Start() {
            // 广播玩家信息
            List<Uid> uids = NetworkUtils.GetAllClientUid();
            NetworkUtils.Broadcast(MessageDef.battle_start_s2c, GetStartMsg);
            
            Network.LockStep.Instance.Start();
        }

        public void AddPlayer(Uid uid) {
            if (LockStep.Instance.IsRunning) {
                NetworkUtils.Send(uid, MessageDef.battle_start_s2c, GetStartMsg(uid));
            } else {
                CheckAutoStart();
            }
        }

        private void CheckAutoStart() {
            List<Uid> uids = NetworkUtils.GetAllClientUid();
            if (uids.Count >= Config.Instance.Network.auto_start_count) {
                Start();
            }
        }
        
        private battle_start_s2c GetStartMsg(Uid selfUid) {
            battle_start_s2c msg = new battle_start_s2c {
                SelfUid = selfUid,
            };
            List<Uid> uids = NetworkUtils.GetAllClientUid();
            foreach (var uid in uids) {
                msg.Uids.Add(uid);
            }
            return msg;
        }
    }
}
