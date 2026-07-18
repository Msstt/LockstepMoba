using Combat.BehaviourMachine;

namespace Combat.Actor {
    public class BehaviourMachineCom : Com {
        private Machine machine;
        private ControlCom controlCom;
        
        public override void Awake() {
            machine = new Machine();
            controlCom = Actor.GetComponent<ControlCom>();
            
            machine.AddCustomEvaluateFunc((behaviour) => {
                if (behaviour is not ActorBehaviour actorBehaviour) {
                    return true;
                }

                if (controlCom?.IsAbort(actorBehaviour.SkillType) == true) {
                    return false;
                }
                return true;
            });
            
            if (Define.create.ContainsKey(Actor.Id)) {
                Define.create[Actor.Id](machine);
            } else {
                Log.Warning($"BehaviourMachineCom Awake: Actor Id {Actor.Id} has no behaviour machine defined.");
            }
        }

        public override void Destroy() {
            machine.Abort();
        }
    }
}