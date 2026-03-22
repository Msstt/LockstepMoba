using System.Collections.Generic;
using Combat.Skill;
using Framework;
using Network;
using UnityEngine;

namespace Combat.Actor {
    public class ActorSystem : IActorSystem {
        public Transform TransRoot { get; private set; }

        private int maxUid = 0;
        
        // 玩家 uid 与 角色系统 uid 一致
        private SafeDictionary<int, Actor> actors = new SafeDictionary<int, Actor>();

        private Dictionary<int, CampType> camp = new Dictionary<int, CampType>();
        
        public void Init() {
            // 玩家的主控会占 10 个
            maxUid = 10;
            TransRoot = new GameObject("[Actor]").transform;
            
            EventMgr.Instance.Register<EventType.OnLockStepStart>(() => {
                NetworkUtils.RegisterHandler<skill_input>(MessageDef.skill_input, SkillHandler);
            });
        }
        
        public void Start() {
            CreateChampion();
        }
        
        public void FrameUpdate(int frame) {
            foreach (var (_, actor) in actors) {
                actor.Update(frame);
            }
        }

        public void Update() {
            foreach (var (_, actor) in actors) {
                actor.RenderUpdate();
            }
        }
        
        private void CreateChampion() {
            ICombatSystem combat = GameMgr.Instance.GetSystem<ICombatSystem>();
            foreach (var uid in combat.PlayerUid) {
                camp[uid] = combat.GetCamp(uid);
            }
            int index = 0;
            foreach (var uid in combat.PlayerUid) {
                CreateActor(new ReviveChampion(uid));
                index++;
            }
        }

        public int GetUid() {
            return ++maxUid;
        }

        public Actor GetActor(int uid) {
            return actors[uid];
        }

        public Actor CreateActor(ActorCreator creator) {
            GameObject go = new GameObject();
            go.transform.SetParent(TransRoot);
            GoUtils.NewGo(creator.PrefabName, go.transform, true).name = "Prefab";

            Actor actor = creator.Create(go);
            actors[actor.Uid] = actor;
            go.name = actor.Type + "-" + actor.Uid;
            actor.BindCom();
            
            NavmeshUtils.RegisterUnit(actor.Uid, (int)actor.Type, actor.Pos, actor.Event.OnChangePos);
            return actor;
        }
        
        public void RemoveActor(int uid) {
            if (!actors.ContainsKey(uid)) {
                return;
            }
            Actor actor = actors[uid];
            NavmeshUtils.UnRegisterUnit(actor.Uid, actor.Event.OnChangePos);
            
            actor.Dispose();
            actors.Remove(uid);
        }
        
        private void SkillHandler(SortedDictionary<Uid, skill_input> inputs) {
            foreach (var (uid, input) in inputs) {
                Actor actor = GetActor(uid);
                if (actor == null) {
                    continue;
                }
                SkillCom com = actor.GetComponent<SkillCom>();
                if (com == null) {
                    Log.Warning("Actor " + uid + " has no SkillCom");
                    continue;
                }
                foreach (var info in input.Info) {
                    com.ExecuteSkill((SkillSlot)info.Slot, new SkillParam(info.Param));
                }
            }
        }

        public bool IsSameCamp(int aUid, int bUid) {
            return camp.GetValueOrDefault(aUid, CampType.UnKnown) == camp.GetValueOrDefault(bUid, CampType.UnKnown);
        }
    }
}