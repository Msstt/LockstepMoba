using Combat;
using Combat.Actor;
using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Actor {
    public struct ChampionStatsBarPanelParam : IUIParam {
        public Combat.Actor.Actor actor;
    }
    
    public class ChampionStatsBarPanel : UIPanel {
        private UIBarCom health;
        private Material healthMat;

        private Combat.Actor.Actor actor;
        
        public override void OnAwake() {
            health = Transform.GetComponent<UIBarCom>("Health");
            
            healthMat = new Material(Shader.Find("UI/BarWithTick"));
            Image healthMax = Transform.Find("Health/Max").GetComponent<Image>();
            healthMax.material = healthMat;
        }

        public override void OnShow(IUIParam uiParam) {
            if (uiParam is not ChampionStatsBarPanelParam param || param.actor == null) {
                return;
            }
            actor = param.actor;
            healthMat.SetColor("_BarColor", ActorUtils.IsSameCamp(actor.Uid) ? "5DD322".ToColor() : "9B2720".ToColor());
            
            actor.Stats.Health.OnValueChanged += RefreshHealth;
        }

        public override void OnHide() {
            actor.Stats.Health.OnValueChanged -= RefreshHealth;
        }

        public override void OnDestroy() {
            if (healthMat != null) {
                GameObject.Destroy(healthMat);
                healthMat = null;
            }
        }

        private void RefreshHealth(FloatF curValue, FloatF maxValue) {
            healthMat?.SetFloat("_TickRange", maxValue.ToFloat());
            health.Value = curValue.ToFloat() / maxValue.ToFloat();
        }
    }
}