using System;
using UnityEngine;

namespace Framework {
    public abstract class Singleton<T> where T : Singleton<T> {
        protected static readonly T instance = Activator.CreateInstance<T>();

        public static T Instance => instance;
    }

    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T> {
        private static T instance;

        private static void Init() {
            GameObject parent = GameObject.Find("/[MonoSingleton]");
            if (parent == null) {
                parent = new GameObject("[MonoSingleton]");
                DontDestroyOnLoad(parent);
            }
            GameObject go = new GameObject(typeof(T).Name);
            go.transform.SetParent(parent.transform);
            instance = go.AddComponent<T>();
        }
        
        public static T Instance {
            get {
                if (instance == null) {
                    Init();
                }
                return instance;
            }
        }
    }
}