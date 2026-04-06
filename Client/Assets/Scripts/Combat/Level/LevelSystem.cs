namespace Combat.Actor {
    public class LevelSystem : ILevelSystem {
        public void Start() {
            EventUtils.Register<EventType.ActorDead>(OnActorDead);
        }

        private void OnActorDead(EventType.ActorDead param) {
            // TODO 助攻
            int level = ActorUtils.GetCom<LevelCom>(param.Uid).Level;
            int exp = Config.Exp.killChampionExp[level];
            ActorUtils.GetCom<LevelCom>(param.KillerUid).AddExp(exp);
        }
    }
}