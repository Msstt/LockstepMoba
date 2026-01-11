using UI;
using UI.Actor;
using UnityEngine;

namespace Combat.Actor {
    public class StatsBarCom : Com {
        private Transform bindingGo;
        
        public override void Awake() {
            bindingGo = Actor.Go.transform.Find("Prefab/StatsBarBindingPoint");
            UIUtils.BindingUI(UIDef.StatsBarPanel, bindingGo, new StatsBarComParam {
                actor = Actor,
            });
        }

        public override void Destroy() {
            UIUtils.UnBindingUI(UIDef.StatsBarPanel, bindingGo);
        }
    }
}