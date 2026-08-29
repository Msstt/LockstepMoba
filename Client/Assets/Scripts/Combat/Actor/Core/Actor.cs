using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using InputSystem;
using UnityEngine;

namespace Combat.Actor {
    public abstract partial class Actor : IDisposable, ICheckableData {
        public int Id { get; private set; }
        public int Uid { get; private set; }
        public ActorType Type { get; protected set; }
        public ActorConfig Config { get; protected set; }
        
        private Dictionary<Type, Com> coms = new Dictionary<Type, Com>();
        private List<Com> comList = new List<Com>();
        public IReadOnlyList<Com> ComList => comList;
        
        private Vector3F pos;
        private Vector3F dir;

        public CampType Camp => camp;
        private CampType camp;

        public GameObject Go => go;
        private GameObject go;

        public Stats Stats = new Stats();
        
        public readonly EventHub Event = new EventHub();

        private GameObject debugPoint;
        
        protected Actor(int id, int uid, GameObject go, CampType camp) {
            Id = id;
            Uid = uid;
            this.go = go;
            this.camp = camp;
#if UNITY_EDITOR
            debugPoint = GoUtils.NewGo("Role/Other/DebugPoint", DebugMgr.Instance.transform);
            debugPoint.SetActive(GameMgr.Instance.GMTool.ShowDebugMode);
#endif
            
            Transform collider = go.transform.Find("Prefab/Collider");
            if (collider == null) {
                throw new CombatException("Actor must have Collider child");
            }
            collider.gameObject.AddComponent<ActorRaycasterCom>().Uid = uid;
        }

        public Vector3F Pos => pos;

        public Vector3F Dir => dir;
        
        #region 组件

        public void AddComponent<T>() where T : Com, new() {
            if (coms.ContainsKey(typeof(T))) {
                Log.Warning($"{Uid} already has component {typeof(T)}");
                return;
            }

            // 先这样吧，感觉怪怪的
            T com = ActorUtils.GetPersistentCom<T>(Uid);
            if (com == null) {
                com = new T();
                if (com is PersistentCom persistentCom) {
                    persistentCom.Uid = Uid;
                } else {
                    com.Actor = this;
                }
            }
            coms[typeof(T)] = com;
            comList.Add(com);
            coms[typeof(T)].Awake();
        }
        
        public void RemoveComponent<T>() where T : Com {
            if (!coms.ContainsKey(typeof(T))) {
                Log.Warning($"{Uid} does not have component {typeof(T)}");
                return;
            }

            coms[typeof(T)].Destroy();
            comList.Remove(coms[typeof(T)]);
            coms.Remove(typeof(T));
        }
        
        public void RemoveAllComponent() {
            foreach (var com in comList) {
                com.Destroy();
            }
            comList.Clear();
            coms.Clear();
        }
        
        public T GetComponent<T>() where T : Com {
            if (coms.ContainsKey(typeof(T))) {
                return coms[typeof(T)] as T;
            }
            return null;
        }
        
        public void Update(int frame) {
            foreach (var com in comList) {
                Profiler.Instance.BeginActorComUpdate(com.GetType());
                com.Update(frame);
                Profiler.Instance.EndActorComUpdate(com.GetType());
            }
        }
        
        private bool? lastVisibility = null;
        
        public void RenderUpdate() {
            foreach (var com in comList) {
                com.RenderUpdate();
            }

            bool nowVisibility = FogUtils.IsVisible(this);
            if (nowVisibility != lastVisibility) {
                // go.SetActive(nowVisibility);
                go.SetVisible(nowVisibility);
                Event.OnVisibilityChangeLocal.Send(nowVisibility);
                lastVisibility = nowVisibility;
            }
        }

        public abstract void BindCom();
        
        #endregion
        
        public void SetPos(Vector3F pos, bool updateGo = false, bool updateY = false) {
            this.pos = pos;
            if (updateGo && go != null) {
                go.transform.position = new Vector3(pos.x.ToFloat(), updateY ? pos.y.ToFloat() : go.transform.position.y, pos.z.ToFloat());
            }
            Event.OnChangePos.Send(pos);

            if (debugPoint) {
                debugPoint.transform.position = pos.ToVector3();
            }
        }
        
        public void SetDir(Vector3F dir, bool updateGo = false) {
            dir.y = 0;
            dir = dir.Normalized();
            if (dir == Vector3F.zero) {
                return;
            }
            this.dir = dir;
            if (updateGo && go != null) {
                go.transform.forward = dir.ToVector3();
            }
        }

        public int Level {
            get {
                LevelCom com = GetComponent<LevelCom>();
                return com?.Level ?? 1;
            }
        }

        private void OnDead() {
            IActorSystem system = GameMgr.Instance.GetSystem<IActorSystem>();
            system?.RemoveActor(Uid);
        }
        
        public void Dispose() {
            RemoveAllComponent();
            GameObject.Destroy(go);
            GameObject.Destroy(debugPoint);
        }

        public int GetStatusCode() {
            int code = StatusCode.Seed;
            code = StatusCode.Combine(code, Id);
            code = StatusCode.Combine(code, Uid);
            code = StatusCode.Combine(code, (int)Type);
            code = StatusCode.Combine(code, (int)Camp);
            code = StatusCode.Combine(code, Pos);
            code = StatusCode.Combine(code, Dir);
            code = StatusCode.CombineData(code, Stats);
            code = StatusCode.Combine(code, comList.Count);
            foreach (var com in comList.OrderBy(com => com.GetType().FullName, StringComparer.Ordinal)) {
                code = StatusCode.CombineType(code, com.GetType());
                code = StatusCode.Combine(code, com.GetStatusCode());
            }
            return code;
        }
    }
}
