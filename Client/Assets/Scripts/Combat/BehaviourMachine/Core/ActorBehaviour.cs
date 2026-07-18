using Combat.Skill;

namespace Combat.BehaviourMachine {
    public abstract class ActorBehaviour : Behaviour {
        public SkillType SkillType { get; }
    }
}