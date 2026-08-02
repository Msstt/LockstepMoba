using Combat.Skill;

namespace Combat.BehaviourMachine {
    public abstract class ActorBehaviour : Behaviour {
        public abstract SkillType SkillType { get; }
        
        protected ActorBehaviour(Machine machine) : base(machine) {
        }
    }
}