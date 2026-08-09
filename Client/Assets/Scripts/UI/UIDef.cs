using System;
using System.Collections.Generic;
using Framework.UI;
using UI.Actor;
using UI.Main;

namespace UI {
    public enum UIDef {
        None,
        ChampionStatsBarPanel,
        MinionStatsBarPanel,
        FloatingNumberPanel,
        SkillPanel,
        LevelPanel,
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
            { UIDef.MinionStatsBarPanel, new Info {
                prefab = "UI/Actor/MinionStatsBarPanel",
                layer = UILayer.World,
                creator = () => new MinionStatsBarPanel(),
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
            { UIDef.LevelPanel, new Info {
                prefab = "UI/Main/LevelPanel",
                layer = UILayer.Back,
                creator = () => new LevelPanel(),
            } },
        };
            
        public class Info {
            public string prefab;
            public UILayer layer;
            public Func<UIPanel> creator;
        }
    }
}