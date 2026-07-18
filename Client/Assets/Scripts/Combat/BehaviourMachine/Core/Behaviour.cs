using Combat.Skill;

namespace Combat.BehaviourMachine {
    public abstract class Behaviour {
        public abstract bool Evaluate();
        public abstract void Execute(int frame);
        
        public abstract void OnStart();
        public abstract void OnAbort();
    }
}