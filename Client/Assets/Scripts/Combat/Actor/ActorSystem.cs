using System.Collections.Generic;
using System.Runtime.InteropServices;
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
                var championId = combat.GetChampionId(uid);
                Champion actor = Champion.Create(championId, combat.GetCamp(uid));
                actor.SetPos(combat.MapConfig.spawnPoint[index], true);
                actor.SetDir(new Vector3F( 1, 0, 0), true);
                actor.Go.transform.position = new Vector3(actor.Go.transform.position.x, combat.MapConfig.spawnPoint[index].y.ToFloat(), actor.Go.transform.position.z);
                actors[actor.Uid] = actor;
                index++;
            }
        }

        public int GetUid() {
            return ++maxUid;
        }

        public Actor GetActor(int uid) {
            return actors[uid];
        }

        public void RemoveActor(int uid) {
            if (!actors.ContainsKey(uid)) {
                return;
            }
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