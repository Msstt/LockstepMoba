using UnityEngine;

namespace Combat.Actor {
    public class TestUnit : Actor {
        public TestUnit(int id, int uid, GameObject go, CampType camp) : base(id, uid, go, camp) {
            Type = ActorType.Test;
        }
        
        public override void BindCom() {
            AddComponent<MoveCom>();
        }
    }
}