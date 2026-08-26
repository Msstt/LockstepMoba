using Framework;
using Network;

namespace Battle {
    public class Match : Singleton<Match> {
        private battle_start_s2c? start_msg = null;
        private Dictionary<int, int> championId = new Dictionary<int, int>();
        
        public void Start() {
            // 广播玩家信息
            NetworkUtils.Broadcast(MessageDef.battle_start_s2c, GetStartMsg);
            
            Network.LockStep.Instance.Start();
        }

        public void AddPlayer(EventType.OnPlayerConnected param) {
            if (LockStep.Instance.IsRunning) {
                NetworkUtils.Send(param.uid, MessageDef.battle_start_s2c, GetStartMsg(param.uid));
            } else {
                CheckAutoStart();
            }
        }
        
        public void SetChampion(Uid uid, int championId) {
            if (LockStep.Instance.IsRunning) {
                return;
            }

            this.championId[uid] = championId;
            NetworkUtils.Send(uid, MessageDef.select_champion_s2c, new select_champion_s2c {
                ChampionId = championId,
            });
            CheckAutoStart();
        }

        private void CheckAutoStart() {
            int count = 0;
            foreach (var uid in NetworkUtils.GetAllClientUid()) {
                if (championId.ContainsKey(uid)) {
                    count += 1;
                }
            }
            if (count >= Config.Instance.Network.auto_start_count) {
                Start();
            }
        }
        
        private battle_start_s2c GetStartMsg(Uid selfUid) {
            if (start_msg != null) {
                start_msg.SelfUid = selfUid;
                return start_msg;
            }
            start_msg = new battle_start_s2c {
                SelfUid = selfUid,
            };
            List<Uid> uids = NetworkUtils.GetAllClientUid();
            int camp = 0;
            foreach (var uid in uids) {
                start_msg.Players.Add(
                    new battle_start_s2c.Types.player_info {
                        Uid = uid,
                        ChampionId = championId[uid],
                        Camp = camp,
                        Skill = { 5, 4 }, // TODO 选技能
                    });
                camp ^= 1;
            }
            return start_msg;
        }
    }
}
