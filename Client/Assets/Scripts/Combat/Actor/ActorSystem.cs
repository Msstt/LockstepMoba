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
        private SortedDictionary<int, Actor> actors = new SortedDictionary<int, Actor>();
        private List<int> toRemove = new List<int>();

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
            foreach (var actor in actors.Values) {
                actor.Update(frame);
            }
            foreach (var uid in toRemove) {
                if (actors.TryGetValue(uid, out Actor actor)) {
                    actor.Clear();
                    actors.Remove(uid);
                }
            }
            toRemove.Clear();
        }

        public void Update() {
            foreach (var actor in actors.Values) {
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
            return actors.GetValueOrDefault(uid, null);
        }

        // 异步删除
        public void RemoveActor(int uid) {
            toRemove.Add(uid);
        }
        
        private void SkillHandler(SortedDictionary<Uid, skill_input> inputs) {
            foreach (var (uid, input) in inputs) {
                Actor actor = GetActor(uid);
                SkillCom com = actor.GetComponent<SkillCom>();
                if (com == null) {
                    Log.Warning("Actor " + uid + " has no SkillCom");
                    continue;
                }
                foreach (var info in input.Info) {
                    com.ExecuteSkill((SkillSlot)info.Slot, new SkillParam(info.Param));
                    // if (info.Slot == (int)SkillSlot.Move) { // TODO 技能树
                    //     MoveCom com = actor.GetComponent<MoveCom>();
                    //     AnimCom ani = actor.GetComponent<AnimCom>();
                    //     com.ForceFail();
                    //     ani.PlayAnim("Run");
                    //     com.MoveToPosByPath(info.Param.Pos.ToVector3F(),
                    //         () => {
                    //             ani.PlayAnim("Idle");
                    //             Debug.Log("Move finished");
                    //         },
                    //         () => {
                    //             ani.PlayAnim("Idle");
                    //             Debug.Log("Move failed");
                    //         });
                    // } else if (info.Slot == (int)SkillSlot.Attack) {
                    //     MoveCom com = actor.GetComponent<MoveCom>();
                    //     AnimCom ani = actor.GetComponent<AnimCom>();
                    //     com.ForceFail();
                    //     ani.PlayAnim("Run");
                    //     com.MoveToActorByPath(info.Param.Uid, actor.Stats.AttackDistance, // TODO radius
                    //         () => {
                    //             ani.PlayAnim("Attack1");
                    //             Actor target = GetActor(info.Param.Uid);
                    //             if (target != null) {
                    //                 target.OnHit(actor.CreateAttackHitInfo());
                    //             }
                    //         },
                    //         () => {
                    //             ani.PlayAnim("Idle");
                    //         });
                    // }
                }
            }
        }

        public bool IsSameCamp(int aUid, int bUid) {
            return camp.GetValueOrDefault(aUid, CampType.UnKnown) == camp.GetValueOrDefault(bUid, CampType.UnKnown);
        }
    }
}