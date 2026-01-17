using UI;
using UI.Actor;
using UnityEngine;

namespace Combat.Actor {
    public class StatsBarCom : Com {
        private Transform bindingGo;

        private UIDef PanelDef {
            get {
                switch (Actor.Type) {
                    case ActorType.Champion:
                        return UIDef.ChampionStatsBarPanel;
                    default:
                        return UIDef.None;
                }
            }
        }
        
        public override void Awake() {
            bindingGo = Actor.Go.transform.Find("Prefab/StatsBarBindingPoint");
            UIUtils.BindingUI(PanelDef, bindingGo, new ChampionStatsBarPanelParam {
                actor = Actor,
            });
        }

        public override void Destroy() {
            UIUtils.UnBindingUI(PanelDef, bindingGo);
        }
    }
}