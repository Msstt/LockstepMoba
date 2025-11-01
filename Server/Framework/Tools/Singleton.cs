using System;

namespace Framework {
    public abstract class Singleton<T> where T : Singleton<T> {
        protected static readonly T instance = Activator.CreateInstance<T>();

        public static T Instance => instance;
    }
}