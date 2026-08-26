namespace Combat.Actor {
    public partial class SpawnSystem {
        public class TurretSpawn {
            private IActorSystem actorSystem;
            
            public void Init() {
                IActorSystem actorSystem = GameMgr.Instance.GetSystem<IActorSystem>();
                if (actorSystem == null) {
                    throw new CombatException("SpawnSystem: ActorSystem not found");
                }
                
                
                foreach (var pos in Config.Map.blueTurretPos) {
                    actorSystem.CreateActor(new CreateTurret(TempConfig.TurretId, CampType.Blue, pos));
                }
                foreach (var pos in Config.Map.redTurretPos) {
                    actorSystem.CreateActor(new CreateTurret(TempConfig.TurretId, CampType.Red, pos));
                }
            }
        }
    }
}