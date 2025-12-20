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
        
        private Dictionary<Type, Com> coms = new Dictionary<Type, Com>(); //TODO +list
        
        private Vector3F pos;
        private FloatF dir;

        private GameObject go;
        
        protected Actor(int uid, GameObject go) {
            Uid = uid;
            this.go = go;
        }

        public Vector3F Pos {
            get => pos;
            set {
                pos = value;
                if (go != null) {
                    go.transform.position = pos.ToVector3();
                }
            }
        }
        
        public FloatF Dir {
            get => dir;
            set {
                dir = value;
                if (go != null) {
                    go.transform.rotation = Quaternion.Euler(0f, dir.ToFloat(), 0f);
                }
            }
        }

        public void AddComponent<T>() where T : Com, new() {
            if (coms.ContainsKey(typeof(T))) {
                Log.Warning($"{Uid} already has component {typeof(T)}");
                return;
            }

            T com = new T();
            com.Actor = this;
            coms[typeof(T)] = com;
        }
        
        public void RemoveComponent<T>() where T : Com {
            if (!coms.ContainsKey(typeof(T))) {
                Log.Warning($"{Uid} does not have component {typeof(T)}");
                return;
            }

            coms[typeof(T)].Destroy();
            coms.Remove(typeof(T));
        }
        
        public void Update() {
            foreach (var com in coms.Values) {
                com.Update();
            }
        }
    }
}