using Combat.Skill;

namespace Combat.BehaviourMachine {
    public abstract partial class Behaviour {
        private Machine machine;

        protected Behaviour(Machine machine) {
            this.machine = machine;
        }
        
        public abstract bool Evaluate();
        public virtual void Execute(int frame) { }
        
        public virtual void OnStart() { }
        public virtual void OnAbort() { }
    }
}