using Framework;

namespace Combat.Actor {
    public class LevelCom : PersistentCom {
        private readonly int MaxLevel = 18;
        
        private ObservableValue<int> level = new ObservableValue<int>(1);
        private ObservableValue<int> exp = new ObservableValue<int>(0);
        
        private int needExp = Config.Exp.upgradeExp[1];
        
        public int Level => level.Value;
        public IReadOnlyObservableValue<int> LevelValue => level;
        public IReadOnlyObservableValue<int> Exp => exp;
        
        public void AddExp(int exp) {
            this.exp.Value += exp;
            while (level.Value < MaxLevel && this.exp.Value >= needExp) {
                this.exp.Value -= needExp;
                Upgrade();
            }
        }

        private void Upgrade() {
            level.Value += 1;
            needExp = Config.Exp.upgradeExp[level.Value];
            
            EventUtils.Send(new EventType.ChampionLevelUp {
                Uid = Uid,
                Level = level.Value,
            });
        }

        public override int GetStatusCode() {
            int code = StatusCode.Combine(StatusCode.Seed, Uid);
            code = StatusCode.Combine(code, level.Value);
            code = StatusCode.Combine(code, exp.Value);
            return StatusCode.Combine(code, needExp);
        }
    }
}
