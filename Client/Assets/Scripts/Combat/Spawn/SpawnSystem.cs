using System.Collections.Generic;
using System.Linq;
using Data;
using Framework;

namespace Combat.Actor {
    public class SpawnSystem : ISpawnSystem {
        private IActorSystem actorSystem;
        private IReadOnlyList<Uid> playerUid;

        private SafeDictionary<int, int> reviveTime = new SafeDictionary<int, int>();
        
        public void Start() {
            actorSystem = GameMgr.Instance.GetSystem<IActorSystem>();
            if (actorSystem == null) {
                Log.Error("SpawnSystem: ActorSystem not found");
            }
            
            EventUtils.Register<EventType.ActorDead>(OnActorDead);
            CreateChampion();
        }

        public void FrameUpdate(int frame) {
            AutoReviveChampion(frame);
        }

        private void CreateChampion() {
            ICombatSystem combat = GameMgr.Instance.GetSystem<ICombatSystem>();
            playerUid = combat.PlayerUid;
            foreach (var uid in playerUid) {
                actorSystem.CreateActor(new ReviveChampion(uid));
            }
        }

        private void OnActorDead(EventType.ActorDead param) {
            if (!playerUid.Contains(param.Uid)) {
                return;
            }
            reviveTime[param.Uid] = TimeUtils.GetFrame(GetReviveTime(param));
        }

        private FloatF GetReviveTime(EventType.ActorDead param) {
            int level = DataUtils.Get<LevelData>()[param.Uid].level;
            return Config.Time.championReviveTime[level];
        }
        
        private void AutoReviveChampion(int frame) {
            foreach (var (uid, time) in reviveTime) {
                if (frame >= time && actorSystem.GetActor(uid) == null) {
                    actorSystem.CreateActor(new ReviveChampion(uid));
                    reviveTime.Remove(uid);
                }
            }
        }
        
        public void ReviveChampion(int uid) {
            if (actorSystem.GetActor(uid) != null) {
                return;
            }
            reviveTime.Remove(uid);
            actorSystem.CreateActor(new ReviveChampion(uid));
        }
    }
}