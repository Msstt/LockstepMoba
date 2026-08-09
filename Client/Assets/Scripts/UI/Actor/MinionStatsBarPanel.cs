using Combat;
using Framework.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Actor {
    public struct MinionStatsBarPanelParam : IUIParam {
        public Combat.Actor.Actor actor;
    }
    
    public class MinionStatsBarPanel : UIPanel {
        private UIBarCom health;

        private Combat.Actor.Actor actor;
        
        public override void OnAwake() {
            health = Transform.GetComponent<UIBarCom>("Health");
        }

        public override void OnShow(IUIParam uiParam) {
            if (uiParam is not MinionStatsBarPanelParam param || param.actor == null) {
                return;
            }
            actor = param.actor;
            Transform.Find("Health/Max").GetComponent<Image>().color = GetHealthBarColor(actor.Uid);
            Transform.Find("Health/Fade").GetComponent<Image>().color = GetHealthBarFadeColor(actor.Uid);
            
            actor.Stats.Health.OnValueChanged += RefreshHealth;
        }

        public override void OnHide() {
            actor.Stats.Health.OnValueChanged -= RefreshHealth;
        }

        public override void OnDestroy() {
        }

        private void RefreshHealth(FloatF curValue, FloatF maxValue) {
            health.Value = curValue.ToFloat() / maxValue.ToFloat();
        }

        private Color GetHealthBarColor(int uid) {
            if (uid == CombatUtils.SelfUid) {
                return "5DD322".ToColor();
            } else if (ActorUtils.IsSameCamp(actor.Uid)) {
                return "408CB3".ToColor();
            } else {
                return "9B2720".ToColor();
            }
        }
        
        private Color GetHealthBarFadeColor(int uid) {
            if (uid == CombatUtils.SelfUid) {
                return "5F171D".ToColor();
            } else if (ActorUtils.IsSameCamp(actor.Uid)) {
                return "5F171D".ToColor();
            } else {
                return "E7CD30".ToColor();
            }
        }
    }
}