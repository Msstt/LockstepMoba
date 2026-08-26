// 生成小兵

using UnityEngine;

namespace Combat.Actor {
    public class CreateTurret : ActorCreator {
        private int id;
        private CampType camp;
        private SimpleTransform transform;

        public CreateTurret(int id, CampType camp, SimpleTransform transform) {
            this.id = id;
            this.camp = camp;
            this.transform = transform;
        }
        
        public override Actor Create(GameObject go) {
            if (camp is CampType.UnKnown or CampType.Neutral) {
                throw new CombatException("Invalid camp for champion: " + camp);
            }
            
            Turret actor = new Turret(id, GetNewUid(), go, camp);
            
            SetStatusByConfig(actor, Config.Turret[id]);
            
            actor.SetPos(transform.position, true, true);
            actor.SetDir(new Vector3F(FloatF.Cos(transform.direction), 0, FloatF.Sin(transform.direction)), true);
            
            return actor;
        }

        public override string PrefabName => camp == CampType.Blue ? Config.Turret[id].bluePrefabName : Config.Turret[id].redPrefabName;
    }
}