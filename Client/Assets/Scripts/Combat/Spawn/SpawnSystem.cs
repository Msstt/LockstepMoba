namespace Combat.Actor {
    public partial class SpawnSystem : ISpawnSystem {
        private ChampionSpawn champion = new ChampionSpawn();
        private MinionSpawn minion = new MinionSpawn();
        private TurretSpawn turret = new TurretSpawn();
        
        public void Start() {
#if UNITY_EDITOR
            if (GameMgr.Instance.GMTool.DisableMinion) {
                minion = null;
            }
            if (GameMgr.Instance.GMTool.DisableTurret) {
                turret = null;
            }
#endif
            champion?.Init();
            minion?.Init();
            turret?.Init();
        }

        public void FrameUpdate(int frame) {
            champion?.AutoReviveChampion(frame);
        }
        
        public void ReviveChampion(int uid) {
            champion?.ReviveChampion(uid);
        }
    }
}