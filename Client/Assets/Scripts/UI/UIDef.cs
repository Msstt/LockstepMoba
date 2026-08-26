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
        TurretStatsBarPanel,
        
        FloatingNumberPanel,
        SkillPanel,
        LevelPanel,
        SelectChampionPanel,
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
            { UIDef.TurretStatsBarPanel, new Info {
                prefab = "UI/Actor/TurretStatsBarPanel",
                layer = UILayer.World,
                creator = () => new TurretStatsBarPanel(),
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
            { UIDef.SelectChampionPanel, new Info {
                prefab = "UI/Prepare/SelectChampionPanel",
                layer = UILayer.Top,
                creator = () => new SelectChampionPanel(),
            } },
        };
            
        public class Info {
            public string prefab;
            public UILayer layer;
            public Func<UIPanel> creator;
        }
    }
}