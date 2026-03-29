using System;
using System.Collections.Generic;
using Framework.UI;
using UI.Actor;

namespace UI {
    public enum UIDef {
        None,
        ChampionStatsBarPanel,
        FloatingNumberPanel,
    }
    
    public enum UILayer {
        None,
        Back,
        Top,
        World,
    }
    
    public static class UIConfig {
        public static readonly Dictionary<UIDef, Info> config = new Dictionary<UIDef, Info>() {
            { UIDef.ChampionStatsBarPanel, new Info {
                prefab = "UI/Actor/ChampionStatsBarPanel",
                layer = UILayer.World,
                creator = () => new ChampionStatsBarPanel(),
            } },
            { UIDef.FloatingNumberPanel, new Info {
                    prefab = "UI/Actor/FloatingNumberPanel",
                    layer = UILayer.World,
                    creator = () => new FloatingNumberPanel(),
            } },
        };
            
        public class Info {
            public string prefab;
            public UILayer layer;
            public Func<UIPanel> creator;
        }
    }
}