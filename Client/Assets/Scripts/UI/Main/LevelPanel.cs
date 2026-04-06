using Combat.Actor;
using Framework.UI;
using TMPro;

namespace UI.Main {
    public class LevelPanel : UIPanel {
        private TMP_Text level;
        private UIBarCom exp;
        private int currentLevel = 1;
        
        public override void OnAwake() {
            level = Transform.GetComponent<TMP_Text>("Level");
            exp = Transform.GetComponent<UIBarCom>("Exp");
        }

        public override void OnShow(IUIParam param) {
            LevelCom com = ActorUtils.GetCom<LevelCom>();
            com.LevelValue.OnValueChanged += RefreshLevel;
            com.Exp.OnValueChanged += RefreshExp;
        }

        public override void OnHide() {
            LevelCom com = ActorUtils.GetCom<LevelCom>();
            com.LevelValue.OnValueChanged -= RefreshLevel;
            com.Exp.OnValueChanged -= RefreshExp;
        }
        
        private void RefreshLevel(int level) {
            currentLevel = level;
            this.level.text = level.ToString();
        }
        
        private void RefreshExp(int exp) {
            int maxExp = Config.Exp.upgradeExp[currentLevel];
            this.exp.Value = (float)exp / maxExp;
        }
    }
}