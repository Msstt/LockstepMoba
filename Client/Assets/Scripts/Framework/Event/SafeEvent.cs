using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework {
    public static class SafeEventTemplate {
        public struct Operation<T> {
            public bool isAdd;
            public T listener;
        };
        
        public static void Register<T>(Queue<Operation<T>> operations, T listener) where T : Delegate {
            operations.Enqueue(new Operation<T> {
                isAdd = true,
                listener = listener,
            });
        }
        
        public static void UnRegister<T>(Queue<Operation<T>> operations, T listener) where T : Delegate {
            operations.Enqueue(new Operation<T> {
                isAdd = false,
                listener = listener,
            });
        }

        public static void HandleOperationQueue<T>(Queue<Operation<T>> operations, ref T listeners) where T : Delegate {
            while (operations.Any()) {
                var operation = operations.Dequeue();
                if (operation.isAdd) {
                    listeners = (T)Delegate.Combine(listeners, operation.listener);
                } else {
                    listeners = (T)Delegate.Remove(listeners, operation.listener);
                }
            }
        }
    }
    
    public interface ISafeEvent {
    }

    public class SafeEvent : ISafeEvent {
        private Action listeners;
        private Queue<SafeEventTemplate.Operation<Action>> operations = new Queue<SafeEventTemplate.Operation<Action>>();
        public void Register(Action listener) => SafeEventTemplate.Register(operations, listener);
        public void UnRegister(Action listener) => SafeEventTemplate.UnRegister(operations, listener);

        public void Send() {
            SafeEventTemplate.HandleOperationQueue(operations, ref listeners);
            listeners?.Invoke();
        }
    }
    
    public class SafeEvent<T1> : ISafeEvent {
        private Action<T1> listeners;
        private Queue<SafeEventTemplate.Operation<Action<T1>>> operations = new Queue<SafeEventTemplate.Operation<Action<T1>>>();
        public void Register(Action<T1> listener) => SafeEventTemplate.Register(operations, listener);
        public void UnRegister(Action<T1> listener) => SafeEventTemplate.UnRegister(operations, listener);

        public void Send(T1 param1) {
            SafeEventTemplate.HandleOperationQueue(operations, ref listeners);
            listeners?.Invoke(param1);
        }
    }
    
    public class SafeEvent<T1, T2> : ISafeEvent {
        private Action<T1, T2> listeners;
        private Queue<SafeEventTemplate.Operation<Action<T1, T2>>> operations = new Queue<SafeEventTemplate.Operation<Action<T1, T2>>>();
        public void Register(Action<T1, T2> listener) => SafeEventTemplate.Register(operations, listener);
        public void UnRegister(Action<T1, T2> listener) => SafeEventTemplate.UnRegister(operations, listener);

        public void Send(T1 param1, T2 param2) {
            SafeEventTemplate.HandleOperationQueue(operations, ref listeners);
            listeners?.Invoke(param1, param2);
        }
    }
    
    public class SafeEvent<T1, T2, T3> : ISafeEvent {
        private Action<T1, T2, T3> listeners;
        private Queue<SafeEventTemplate.Operation<Action<T1, T2, T3>>> operations = new Queue<SafeEventTemplate.Operation<Action<T1, T2, T3>>>();
        public void Register(Action<T1, T2, T3> listener) => SafeEventTemplate.Register(operations, listener);
        public void UnRegister(Action<T1, T2, T3> listener) => SafeEventTemplate.UnRegister(operations, listener);

        public void Send(T1 param1, T2 param2, T3 param3) {
            SafeEventTemplate.HandleOperationQueue(operations, ref listeners);
            listeners?.Invoke(param1, param2, param3);
        }
    }
}