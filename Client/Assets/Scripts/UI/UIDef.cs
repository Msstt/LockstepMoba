using System;
using System.Collections.Generic;
using Framework.UI;
using UI.Actor;

namespace UI {
    public enum UIDef {
        None,
        StatsBarPanel,
    }
    
    public enum UILayer {
        None,
        Back,
        Top,
        World,
    }
    
    public static class UIConfig {
        public static readonly Dictionary<UIDef, Info> config = new Dictionary<UIDef, Info>() {
            { UIDef.StatsBarPanel, new Info {
                prefab = "UI/Actor/StatsBarPanel",
                layer = UILayer.World,
                creator = () => new StatsBarPanel(),
            } }
        };
            
        public class Info {
            public string prefab;
            public UILayer layer;
            public Func<UIPanel> creator;
        }
    }
}