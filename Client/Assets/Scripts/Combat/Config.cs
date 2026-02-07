using System;
using System.Collections.Generic;
using Combat.Actor;
using Combat.Buff;
using Combat.Skill;
using Framework;
using UnityEngine;

namespace Combat {
    public static class Config {
        public class ConfigCache<T> {
            private Dictionary<int, T> cache = new Dictionary<int, T>();
            private Func<int, T> loader;

            public ConfigCache(Func<int, T> loader) {
                if (loader == null) {
                    throw new ArgumentNullException(nameof(loader));
                }
                this.loader = loader;
            }
        
            public T this[int id] {
                get {
                    if (cache.ContainsKey(id)) {
                        return cache[id];
                    }
                    T config = loader(id);
                    if (config == null) { 
                        throw new CombatException($"{typeof(T)} config not found: {id}");
                    }
                    return cache[id] = config;
                }
            }
        }

        public static readonly ConfigCache<ChampionConfig> Champion = new(
            (id) => Resources.Load<ChampionConfig>("Config/Actor/Champion/" + id));
        
        public static readonly ConfigCache<SkillConfig> Skill = new(
            (id) => JsonHelper.LoadFromRes("Config/Skill/Json/" + id, out SkillConfig config) ? config : null);
        
        public static readonly ConfigCache<BuffConfig> Buff = new(
            (id) => JsonHelper.LoadFromRes("Config/Buff/Json/" + id, out BuffConfig config) ? config : null);
    }
}