using UnityEngine;

namespace Combat.Actor {
    public class CreateTestUnit : ActorCreator {
        private Transform pos;
        private CampType camp;

        public CreateTestUnit(Transform pos, CampType camp = CampType.UnKnown) {
            this.pos = pos;
            this.camp = camp;
        }
        
        public override Actor Create(GameObject go) {
            TestUnit actor = new TestUnit(TempConfig.TestUnitActorId, GetNewUid(), go, camp);

            actor.Stats.MoveSpeed = new Priority(0.8f.ToFloatF());
            actor.Stats.Radius = new Priority(0.5f.ToFloatF());
            actor.SetPos(pos.position.ToVector3F(), true, true);
            actor.SetDir(new Vector3F(FloatF.Cos(pos.eulerAngles.y.ToFloatF()), 0, FloatF.Sin(pos.eulerAngles.y.ToFloatF())), true);
            
            return actor;
        }

        public override string PrefabName => "Role/Other/TestUnit";

    }
}