namespace Combat.Actor {
    public class LevelSystem : ILevelSystem {
        public void Start() {
            EventUtils.Register<EventType.ActorDead>(OnActorDead);
        }

        private void OnActorDead(EventType.ActorDead param) {
            // TODO 助攻
            switch (param.Type) {
                case ActorType.Champion:
                    OnChampionDead(param);
                    break;
                default:
                    break;
            }
        }

        private void OnChampionDead(EventType.ActorDead param) {
            int level = ActorUtils.GetCom<LevelCom>(param.Uid).Level;
            int exp = Config.Exp.killChampionExp[level];
            ActorUtils.GetCom<LevelCom>(param.KillerUid)?.AddExp(exp);
        }
    }
}