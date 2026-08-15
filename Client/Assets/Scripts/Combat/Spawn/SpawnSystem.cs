namespace Combat.Actor {
    public partial class SpawnSystem : ISpawnSystem {
        private ChampionSpawn champion = new ChampionSpawn();
        private MinionSpawn minion = new MinionSpawn();
        
        public void Start() {
            champion.Init();
            minion.Init();
        }

        public void FrameUpdate(int frame) {
            champion.AutoReviveChampion(frame);
        }
        
        public void ReviveChampion(int uid) {
            champion.ReviveChampion(uid);
        }
    }
}