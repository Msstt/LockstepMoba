using System;
using System.Collections.Generic;
using Framework.UI;
using UI.Actor;
using UI.Main;

namespace UI {
    public enum UIDef {
        None,
        ChampionStatsBarPanel,
        FloatingNumberPanel,
        SkillPanel,
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
            { UIDef.SkillPanel, new Info {
                prefab = "UI/Main/SkillPanel",
                layer = UILayer.Back,
                creator = () => new SkillPanel(),
            } },
        };
            
        public class Info {
            public string prefab;
            public UILayer layer;
            public Func<UIPanel> creator;
        }
    }
}