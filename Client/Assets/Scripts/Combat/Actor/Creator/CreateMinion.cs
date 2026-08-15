using UnityEngine;

namespace Combat.Actor {
    public class CreateMinion : ActorCreator {
        private int id;
        private CampType camp;
        private int waveIndex;

        public CreateMinion(int id, CampType camp, int waveIndex) {
            this.id = id;
            this.camp = camp;
            this.waveIndex = waveIndex;
        }
        
        public override Actor Create(GameObject go) {
            if (camp is CampType.UnKnown or CampType.Neutral) {
                throw new CombatException("Invalid camp for champion: " + camp);
            }
            
            Minion actor = new Minion(id, GetNewUid(), go, camp);
            
            SetStatusByConfig(actor, id);

            var waves = camp == CampType.Blue ? Config.Map.blueMinionWavePos : Config.Map.redMinionWavePos;
            if (waveIndex >= waves.Count || waves[waveIndex].Pos.Count == 0) {
                throw new CombatException("Invalid waveIndex for minion: " + waveIndex);
            }
            var transform = waves[waveIndex].Pos[0];
            actor.SetPos(transform.position, true, true);
            actor.SetDir(new Vector3F(FloatF.Cos(transform.direction), 0, FloatF.Sin(transform.direction)), true);
            
            return actor;
        }

        public override string PrefabName => camp == CampType.Blue ? Config.Minion[id].bluePrefabName : Config.Minion[id].redPrefabName;
        
        private void SetStatusByConfig(Actor actor, int Id) {
            MinionConfig config = Config.Minion[Id];
            
            actor.Stats.Health = new LimitedPriority(config.health[1]);
            
            actor.Stats.Attack = new Priority(config.attack[1]);
            actor.Stats.AttackSpeed = new Priority(config.attackSpeed);
            actor.Stats.AttackDistance = new Priority(config.attackDistance);
            
            actor.Stats.MoveSpeed = new Priority(config.moveSpeed);
        }
    }
}