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
            actor.Stats.MoveSpeed = config.moveSpeed;
            return actor;
        }
        
        public Champion(int uid, GameObject go) : base(uid, go) {
            Type = ActorType.Champion;
            
            AddComponent<MoveCom>();
            AddComponent<AnimCom>();
            AddComponent<SlotCom>();
        }
    }
}