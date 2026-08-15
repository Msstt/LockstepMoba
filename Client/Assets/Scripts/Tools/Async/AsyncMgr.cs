// 基于 frame 的异步器

using System;
using System.Collections.Generic;
using Framework;

public class AsyncMgr : Singleton<AsyncMgr> {
    private class Info {
        public int frame;
        public Action func;
    }
    
    PriorityQueue<int, Info> queue = new PriorityQueue<int, Info>(Comparer<Info>.Create((x, y) => x.frame - y.frame));
    HashSet<int> cancelKey = new HashSet<int>();
    HashSet<int> pendingKey = new HashSet<int>();
    private int maxKey = 0;
    
    public ReleaseToken Start(int frame, Action func) {
        int key = ++maxKey;
        queue.Enqueue(key, new Info {
            frame = frame,
            func = func,
        });
        pendingKey.Add(key);
        return new ReleaseToken(() => Stop(key));
    }

    private void Stop(int key) {
        if (pendingKey.Contains(key)) {
            cancelKey.Add(key);
        }
    }

    public void Update(int frame) {
        while (queue.Count > 0) {
            queue.Dequeue(out int key, out Info info);
            if (cancelKey.Contains(key)) {
                cancelKey.Remove(key);
                pendingKey.Remove(key);
                continue;
            }
            if (info.frame > frame) {
                queue.Enqueue(key, info);
                break;
            }
            info.func?.Invoke();
            pendingKey.Remove(key);
        }
    }
}
