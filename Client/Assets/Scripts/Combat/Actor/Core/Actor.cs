using System;
using System.Collections.Generic;
using Framework;
using InputSystem;
using UnityEngine;

namespace Combat.Actor {
    public enum ActorType {
        Champion,
    }
    
    public abstract partial class Actor {
        public int Id { get; private set; }
        public int Uid { get; private set; }
        public ActorType Type { get; protected set; }
        
        private Dictionary<Type, Com> coms = new Dictionary<Type, Com>();
        private List<Com> comList = new List<Com>();
        
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

            debugPoint = GoUtils.NewGo("Role/Other/DebugPoint", DebugMgr.Instance.transform);
            debugPoint.SetActive(GameMgr.Instance.GMTool.ShowUnitRealPos);
            
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

            T com = new T();
            com.Actor = this;
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
                com.Update(frame);
            }
        }
        
        public void RenderUpdate() {
            foreach (var com in comList) {
                com.RenderUpdate();
            }
        }
        
        #endregion
        
        public void SetPos(Vector3F pos, bool updateGo = false) {
            this.pos = pos;
            if (updateGo && go != null) {
                go.transform.position = new Vector3(pos.x.ToFloat(), go.transform.position.y, pos.z.ToFloat());
            }

            debugPoint.transform.position = pos.ToVector3();
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
            Clear();
            IActorSystem system = GameMgr.Instance.GetSystem<IActorSystem>();
            system?.RemoveActor(Uid);
        }
        
        private void Clear() {
            RemoveAllComponent();
            GameObject.Destroy(go);
            GameObject.Destroy(debugPoint);
        }
    }
}