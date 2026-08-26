// 复活英雄

using UnityEngine;

namespace Combat.Actor {
    public class ReviveChampion : ActorCreator {
        private int uid;

        public ReviveChampion(int uid) {
            this.uid = uid;
        }
        
        public override Actor Create(GameObject go) {
            int championId = CombatUtils.GetChampionId(uid);
            CampType camp = CombatUtils.GetCamp(uid);
            if (camp is CampType.UnKnown or CampType.Neutral) {
                throw new CombatException("Invalid camp for champion: " + camp);
            }
            
            Champion actor = new Champion(championId, uid, go, camp);
            
            SetStatusByConfig(actor, Config.Champion[championId]);

            // 暂时先 uid 对应下标
            var pos = Config.Map.revivePos[uid - 1];
            actor.SetPos(pos.position, true, true);
            actor.SetDir(new Vector3F(FloatF.Cos(pos.direction), 0, FloatF.Sin(pos.direction)), true);
            
            return actor;
        }

        public override string PrefabName {
            get {
                int championId = CombatUtils.GetChampionId(uid);
                return Config.Champion[championId].prefabName;
            }
        }
    }
}