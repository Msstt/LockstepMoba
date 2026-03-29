using UI;
using UI.Actor;
using UnityEngine;

namespace Combat.Actor {
    public class NormalUICom : Com {
        private Transform bindingGo;

        private UIDef StatsPanelDef {
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
            UIUtils.BindingUI(StatsPanelDef, bindingGo, new ChampionStatsBarPanelParam {
                actor = Actor,
            });
            UIUtils.BindingUI(UIDef.FloatingNumberPanel, bindingGo, new FloatingNumberPanelParam {
                actor = Actor,
            });
        }

        public override void Destroy() {
            UIUtils.UnBindingUI(StatsPanelDef, bindingGo);
            UIUtils.UnBindingUI(UIDef.FloatingNumberPanel, bindingGo);
        }
    }
}