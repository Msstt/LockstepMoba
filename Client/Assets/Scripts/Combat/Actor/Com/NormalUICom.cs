using Framework.UI;
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
        
        private IUIParam StatsPanelParam {
            get {
                switch (Actor.Type) {
                    case ActorType.Champion:
                        return new ChampionStatsBarPanelParam {
                            actor = Actor,
                        };
                    default:
                        return null;
                }
            }
        }
        
        public override void Awake() {
            Actor.Event.OnVisibilityChange.Register(OnVisibilityChange);
            
            bindingGo = Actor.Go.transform.Find("Prefab/StatsBarBindingPoint");
            UIUtils.BindingUI(StatsPanelDef, bindingGo, StatsPanelParam);
            UIUtils.BindingUI(UIDef.FloatingNumberPanel, bindingGo, new FloatingNumberPanelParam {
                actor = Actor,
            });
        }

        public override void Destroy() {
            UIUtils.UnBindingUI(StatsPanelDef, bindingGo);
            UIUtils.UnBindingUI(UIDef.FloatingNumberPanel, bindingGo);
            
            Actor.Event.OnVisibilityChange.UnRegister(OnVisibilityChange);
        }

        private void OnVisibilityChange(bool visible) {
            bindingGo.GetComponent<BindingUISource>().SetVisible(visible);
        }
    }
}