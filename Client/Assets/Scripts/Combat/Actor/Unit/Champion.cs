using UnityEngine;

namespace Combat.Actor {
    public class Champion : Actor {
        public static Champion Create(int championId) {
            ChampionConfig config = Config.Champion[championId];
            GameObject go = new GameObject("Champion_" + championId);
            go.transform.SetParent(ActorMgr.Instance.TransRoot);
            GoUtils.NewGo(config.prefabName, go.transform, true);
            
            Champion actor = new Champion(ActorMgr.Instance.GetUid(), go);
            return actor;
        }
        
        public Champion(int uid, GameObject go) : base(uid, go) {
            Type = ActorType.Champion;
        }
    }
}