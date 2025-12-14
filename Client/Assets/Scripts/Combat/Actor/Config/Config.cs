using System.Collections.Generic;
using UnityEngine;

namespace Combat.Actor {
    public static class Config {
        public class ChampionConfigCache {
            private Dictionary<int, ChampionConfig> champion = new Dictionary<int, ChampionConfig>();
        
            public ChampionConfig this[int championId] {
                get {
                    if (champion.ContainsKey(championId)) {
                        return champion[championId];
                    }
                    ChampionConfig config = Resources.Load<ChampionConfig>("Config/Actor/Champion/" + championId);
                    if (config == null) {
                        Log.Error($"Champion config not found: {championId}");
                    }
                    champion[championId] = config;
                    return config;
                }
            }
        }

        public static ChampionConfigCache Champion = new ChampionConfigCache();
    }
}