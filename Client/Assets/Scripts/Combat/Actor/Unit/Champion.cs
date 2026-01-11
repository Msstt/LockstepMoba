using UnityEngine;

namespace Combat.Actor {
    public class Champion : Actor {
        public static Champion Create(int championId) {
            IActorSystem system = GameMgr.Instance.GetSystem<IActorSystem>();
            
            ChampionConfig config = Config.Champion[championId];
            GameObject go = new GameObject("Champion_" + championId);
            go.transform.SetParent(system.TransRoot);
            GoUtils.NewGo(config.prefabName, go.transform, true).name = "Prefab";
            
            Champion actor = new Champion(system.GetUid(), go);
            actor.SetStatusByConfig(config);
            return actor;
        }
        
        public Champion(int uid, GameObject go) : base(uid, go) {
            Type = ActorType.Champion;
            
            AddComponent<MoveCom>();
            AddComponent<AnimCom>();
            AddComponent<SlotCom>();
            AddComponent<LevelCom>();
            // AddComponent<StatsBarCom>();
        }

        private void SetStatusByConfig(ChampionConfig config) {
            Stats.Health = new LimitedPriority(config.health[1]);
            Stats.Attack = new Priority(config.attack[1]);
            Stats.AttackSpeed = new Priority(config.attackSpeed[1]);
            Stats.MoveSpeed = new Priority(config.moveSpeed);
        }
    }
}