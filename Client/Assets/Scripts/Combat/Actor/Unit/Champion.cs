using UnityEngine;

namespace Combat.Actor {
    public class Champion : Actor {
        public static Champion Create(int championId, CampType camp) {
            IActorSystem system = GameMgr.Instance.GetSystem<IActorSystem>();
            
            ChampionConfig config = Config.Champion[championId];
            GameObject go = new GameObject("Champion_" + championId);
            go.transform.SetParent(system.TransRoot);
            GoUtils.NewGo(config.prefabName, go.transform, true).name = "Prefab";
            
            Champion actor = new Champion(system.GetUid(), go, camp);
            actor.SetStatusByConfig(config);
            actor.Const = new Const(config);
            actor.BindCom();
            actor.GetComponent<SkillCom>()?.SetSkillId(config);
            return actor;
        }
        
        private Champion(int uid, GameObject go, CampType camp) : base(uid, go, camp) {
            Type = ActorType.Champion;
        }

        private void SetStatusByConfig(ChampionConfig config) {
            Stats.Health = new LimitedPriority(config.health[1]);
            
            Stats.Attack = new Priority(config.attack[1]);
            Stats.AttackSpeed = new Priority(config.attackSpeed[1]);
            Stats.AttackDistance = new Priority(config.attackDistance);
            
            Stats.MoveSpeed = new Priority(config.moveSpeed);
        }
        
        private void BindCom() {
            AddComponent<MoveCom>();
            AddComponent<AnimCom>();
            AddComponent<SlotCom>();
            AddComponent<LevelCom>();
            AddComponent<SkillCom>();
            AddComponent<StatsBarCom>();
        }
    }
    
    public partial class Const {
        public Const(ChampionConfig config) {
            AttackWindupRatio = config.attackWindupRatio;
        }
    }
}