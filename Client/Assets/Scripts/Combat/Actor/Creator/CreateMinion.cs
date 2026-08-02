using UnityEngine;

namespace Combat.Actor {
    public class CreateMinion : ActorCreator {
        private int id;
        private CampType camp;

        public CreateMinion(int id, CampType camp) {
            this.id = id;
            this.camp = camp;
        }
        
        public override Actor Create(GameObject go) {
            if (camp is CampType.UnKnown or CampType.Neutral) {
                throw new CombatException("Invalid camp for champion: " + camp);
            }
            
            Minion actor = new Minion(id, GetNewUid(), go, camp);
            
            SetStatusByConfig(actor, id);

            // TEST
            var pos = Config.Map.revivePos[1];
            actor.SetPos(pos.position, true, true);
            actor.SetDir(new Vector3F(FloatF.Cos(pos.direction), 0, FloatF.Sin(pos.direction)), true);
            
            return actor;
        }

        public override string PrefabName => Config.Minion[id].prefabName;
        
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