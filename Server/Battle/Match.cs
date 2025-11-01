using Framework;
using Network;

namespace Battle {
    public class Match : Singleton<Match> {
        public void Start() {
            // 广播玩家信息
            List<Uid> uids = NetworkUtils.GetAllClientUid();
            NetworkUtils.Broadcast(MessageDef.battle_start_s2c, (selfUid) => {
                battle_start_s2c msg = new battle_start_s2c {
                    SelfUid = selfUid,
                };
                foreach (var uid in uids) {
                    msg.Uids.Add(uid);
                }
                return msg;
            });
            
            Network.LockStep.Instance.Start();
        }

        public void AddPlayer(Uid uid) {
            CheckAutoStart();
        }

        private void CheckAutoStart() {
            List<Uid> uids = NetworkUtils.GetAllClientUid();
            if (uids.Count >= Config.Instance.Network.auto_start_count) {
                Start();
            }
        }
    }
}
