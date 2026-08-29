using System;
using System.Collections.Generic;
using System.Linq;
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
        
        private Dictionary<int, Dictionary<Type, PersistentCom>> persistentComs = new Dictionary<int, Dictionary<Type, PersistentCom>>();
        
        public void Init() {
            // 玩家的主控会占 10 个
            maxUid = 10;
            TransRoot = new GameObject("[Actor]").transform;
            
            EventMgr.Instance.Register<EventType.OnLockStepStart>(() => {
                NetworkUtils.RegisterHandler<skill_input>(MessageDef.skill_input, SkillHandler);
            });
            EventMgr.Instance.Register<EventType.OnLockStepStart>(() => {
                NetworkUtils.RegisterHandler<level_input>(MessageDef.level_input, LevelHandler);
            });
        }
        
        public void Start() {
        }
        
        public void FrameUpdate(int frame) {
            foreach (var (_, actor) in actors) {
                actor.Update(frame);
            }

            foreach (var (uid, coms) in persistentComs) {
                if (GetActor(uid) != null) {
                    continue;
                }
                foreach (var (_, com) in coms) {
                    Profiler.Instance.BeginActorComUpdate(com.GetType());
                    com.Update(frame);
                    Profiler.Instance.EndActorComUpdate(com.GetType());
                }
            }
        }

        public void Update() {
            foreach (var (_, actor) in actors) {
                actor.RenderUpdate();
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
            foreach (var com in actor.ComList) {
                if (com is PersistentCom persistentCom) {
                    RegisterPersistentCom(persistentCom);
                }
            }
            
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

        private void LevelHandler(SortedDictionary<Uid, level_input> inputs) {
            foreach (var (uid, input) in inputs) {
                SkillCom com = GetPersistentCom<SkillCom>(uid);
                foreach (var info in input.LevelUp) {
                    com?.LevelUpSkill((SkillSlot)info.Slot);
                }
            }
        }

        private CampType GetCamp(int uid) {
            return GetActor(uid)?.Camp ?? CampType.UnKnown;
        }

        public bool IsSameCamp(int aUid, int bUid) {
            return GetCamp(aUid) == GetCamp(bUid);
        }

        private void RegisterPersistentCom(PersistentCom com) {
            if (!persistentComs.ContainsKey(com.Uid)) {
                persistentComs[com.Uid] = new Dictionary<Type, PersistentCom>();
            }
            if (!persistentComs[com.Uid].ContainsKey(com.GetType())) {
                persistentComs[com.Uid].Add(com.GetType(), com);
            }
        }

        public T GetPersistentCom<T>(int uid) where T : Com {
            if (persistentComs.TryGetValue(uid, out var comDict) && comDict.TryGetValue(typeof(T), out var com)) {
                return com as T;
            }
            return GetActor(uid)?.GetComponent<T>();
        }

        public int GetStatusCode() {
            int statusCode = StatusCode.Seed;
            var actorList = new List<(int uid, Actor actor)>();
            foreach (var pair in actors) {
                actorList.Add(pair);
            }
            statusCode = StatusCode.Combine(statusCode, actorList.Count);
            foreach (var (uid, actor) in actorList.OrderBy(pair => pair.uid)) {
                statusCode = StatusCode.Combine(statusCode, uid);
                statusCode = StatusCode.CombineData(statusCode, actor);
            }

            var deadPersistentComs = persistentComs
                .Where(pair => GetActor(pair.Key) == null)
                .OrderBy(pair => pair.Key)
                .ToList();
            statusCode = StatusCode.Combine(statusCode, deadPersistentComs.Count);
            foreach (var (uid, coms) in deadPersistentComs) {
                statusCode = StatusCode.Combine(statusCode, uid);
                statusCode = StatusCode.Combine(statusCode, coms.Count);
                foreach (var pair in coms.OrderBy(pair => pair.Key.FullName, StringComparer.Ordinal)) {
                    statusCode = StatusCode.CombineType(statusCode, pair.Key);
                    statusCode = StatusCode.Combine(statusCode, pair.Value.GetStatusCode());
                }
            }
            return statusCode;
        }
    }
}
