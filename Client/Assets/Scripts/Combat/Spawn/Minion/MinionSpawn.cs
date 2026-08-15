namespace Combat.Actor {
    public partial class SpawnSystem {
        public class MinionSpawn {
            private IActorSystem actorSystem;
            private int waveIndex = 0;
            
            public void Init() {
                actorSystem = GameMgr.Instance.GetSystem<IActorSystem>();
                if (actorSystem == null) {
                    throw new CombatException("SpawnSystem: ActorSystem not found");
                }

                if (Config.MinionWave.spawnIntervalTime < 1) {
                    throw new CombatException("MinionWave spawnIntervalTime is too small");
                }
                
                AsyncUtils.Start(Config.MinionWave.spawnStartTime, SpawnMinionWave);
            }
            
            private void SpawnMinionWave() {
                var ids = Config.MinionWave.spawnId[waveIndex];
                FloatF intervalTime = Config.MinionWave.singleIntervalTime;
                for (int i = 0; i < ids.Count; i++) {
                    int id = ids[i];
                    AsyncUtils.Start(i * intervalTime, () => {
                        SpawnMinion(id);
                    });
                }
                waveIndex = (waveIndex + 1) % Config.MinionWave.spawnId.Count;
                
                AsyncUtils.Start(Config.MinionWave.spawnIntervalTime, SpawnMinionWave);
            }

            private void SpawnMinion(int minionId) {
                var blueMinionWave = Config.Map.blueMinionWavePos;
                for (int i = 0; i < blueMinionWave.Count; i++) {
                    actorSystem.CreateActor(new CreateMinion(minionId, CampType.Blue, i));
                }
                var redMinionWave = Config.Map.redMinionWavePos;
                for (int i = 0; i < blueMinionWave.Count; i++) {
                    actorSystem.CreateActor(new CreateMinion(minionId, CampType.Red, i));
                }
            }
        }
    }
}