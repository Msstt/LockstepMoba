using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Framework {
    public enum ThreadTaskId {
        SocketWrite = 1,
        SocketRead = 2,
    }
    
    public class ThreadMgr : Singleton<ThreadMgr> {
        public Dictionary<ThreadTaskId, Action> tasks = new Dictionary<ThreadTaskId, Action>() {
            {ThreadTaskId.SocketWrite, null },
            {ThreadTaskId.SocketRead, null },
        };
        public ConcurrentDictionary<ThreadTaskId, bool> taskIsRunning = new ConcurrentDictionary<ThreadTaskId, bool>();
        
        public ThreadMgr() {
            foreach (ThreadTaskId value in Enum.GetValues(typeof(ThreadTaskId))) {
                taskIsRunning.TryAdd(value, false);
            }
        }
        
        public void Start(ThreadTaskId taskId, Action func) {
            tasks[taskId] = func;
            if (taskIsRunning[taskId]) {
                return;
            }
            taskIsRunning[taskId] = true;
            void Task(object _) {
                tasks[taskId]?.Invoke();
                if (taskIsRunning[taskId]) {
                    ThreadPool.QueueUserWorkItem(Task);
                }
            }
            ThreadPool.QueueUserWorkItem(Task);
        }
        
        public void Stop(ThreadTaskId taskId) {
            taskIsRunning[taskId] = false;
        }
    }
}