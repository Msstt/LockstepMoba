namespace Combat.Actor {
    public class LevelCom : PersistentCom {
        private readonly int MaxLevel = 18;
        
        private int level = 1;
        private int exp = 0;
        
        private int needExp = Config.Exp.upgradeExp[1];
        
        public int Level => level;
        
        public void AddExp(int exp) {
            this.exp += exp;
            while (level < MaxLevel && this.exp >= needExp) {
                this.exp -= needExp;
                Upgrade();
            }
        }

        private void Upgrade() {
            level += 1;
            needExp = Config.Exp.upgradeExp[level];
            
            EventUtils.Send(new EventType.ChampionLevelUp {
                Uid = Uid,
                Level = level,
            });
        }
    }
}