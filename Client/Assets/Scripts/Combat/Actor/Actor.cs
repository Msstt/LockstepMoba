using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.Actor {
    public enum ActorType {
        Champion,
    }
    
    public abstract class Actor {
        public int Uid { get; private set; }
        public ActorType Type { get; protected set; }
        
        private Dictionary<Type, Com> coms = new Dictionary<Type, Com>();
        private List<Com> comList = new List<Com>();
        
        private Vector3F pos;
        private Vector3F dir;

        public GameObject Go => go;
        private GameObject go;

        public Stats Stats;
        
        protected Actor(int uid, GameObject go) {
            Uid = uid;
            this.go = go;
        }

        public Vector3F Pos => pos;

        public Vector3F Dir => dir;

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
        
        public void SetPos(Vector3F pos, bool updateGo = false) {
            this.pos = pos;
            if (updateGo && go != null) {
                go.transform.position = new Vector3(pos.x.ToFloat(), go.transform.position.y, pos.z.ToFloat());
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
    }
}