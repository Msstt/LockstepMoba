namespace Combat.Actor {
    public class LevelSystem : ILevelSystem {
        public void Start() {
            EventUtils.Register<EventType.ActorDead>(OnActorDead);
        }

        private void OnActorDead(EventType.ActorDead param) {
        }
    }
}