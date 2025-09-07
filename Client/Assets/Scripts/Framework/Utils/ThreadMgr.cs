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
            {ThreadTaskId.SocketWrite, () => Network.Network.Instance.FlushWrite() },
            {ThreadTaskId.SocketRead, () => Network.Network.Instance.FlushRead() },
        };
        public ConcurrentDictionary<ThreadTaskId, bool> taskIsRunning = new ConcurrentDictionary<ThreadTaskId, bool>();
        
        public ThreadMgr() {
            for (int i = 1; i <= Enum.GetValues(typeof(ThreadTaskId)).Length; i++) {
                taskIsRunning.TryAdd((ThreadTaskId)i, false);
            }
        }
        
        public void Start(ThreadTaskId taskId) {
            if (taskIsRunning[taskId]) {
                return;
            }
            taskIsRunning[taskId] = true;
            void Task(object _) {
                tasks[taskId]();
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