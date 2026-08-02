namespace Combat.BehaviourMachine {
    public abstract partial class Behaviour {
        protected Actor.Actor Actor => ActorUtils.GetActor(machine.Uid);
    }
}