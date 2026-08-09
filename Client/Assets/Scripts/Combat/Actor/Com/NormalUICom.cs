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
                    case ActorType.Minion:
                        return UIDef.MinionStatsBarPanel;
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
                    case ActorType.Minion:
                        return new MinionStatsBarPanelParam {
                            actor = Actor,
                        };
                    default:
                        return null;
                }
            }
        }
        
        public override void Awake() {
            Actor.Event.OnVisibilityChangeLocal.Register(OnVisibilityChangeLocal);
            
            bindingGo = Actor.Go.transform.Find("Prefab/StatsBarBindingPoint");
            UIUtils.BindingUI(StatsPanelDef, bindingGo, StatsPanelParam);
            UIUtils.BindingUI(UIDef.FloatingNumberPanel, bindingGo, new FloatingNumberPanelParam {
                actor = Actor,
            });
        }

        public override void Destroy() {
            UIUtils.UnBindingUI(StatsPanelDef, bindingGo);
            UIUtils.UnBindingUI(UIDef.FloatingNumberPanel, bindingGo);
            
            Actor.Event.OnVisibilityChangeLocal.UnRegister(OnVisibilityChangeLocal);
        }

        private void OnVisibilityChangeLocal(bool visible) {
            bindingGo.GetComponent<BindingUISource>().SetVisible(visible);
        }
    }
}