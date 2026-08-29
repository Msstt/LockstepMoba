using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityProfiler = UnityEngine.Profiling.Profiler;

namespace Tools.Debug {
    public class FPSDrawer : MonoBehaviour {
        private const float UPDATE_INTERVAL = 1f;
        private const float RECORD_INTERVAL = 10f;
        private const float BYTES_PER_MB = 1024f * 1024f;
        
        private LinkedList<(float, int)> record = new LinkedList<(float, int)>();

        private Text text;
        private float lastUpdateTime;
        private int lastUnityFrame;

        public void Awake() {
            text = GetComponent<Text>();
            lastUpdateTime = Time.realtimeSinceStartup;
            lastUnityFrame = Time.frameCount;
        }

        public void Update() {
            float now = Time.realtimeSinceStartup;
            record.AddLast((now, GameMgr.Instance.Frame));
            while (record.Any() && record.First.Value.Item1 < now - RECORD_INTERVAL) {
                record.RemoveFirst();
            }

            float updateDuration = now - lastUpdateTime;
            if (updateDuration < UPDATE_INTERVAL) {
                return;
            }

            int logicFps = 0;
            if (record.Any() && record.Last.Value.Item1 - record.First.Value.Item1 > 0) {
                var first = record.First.Value;
                var last = record.Last.Value;
                logicFps = Mathf.RoundToInt((last.Item2 - first.Item2) / (last.Item1 - first.Item1));
            }

            int unityFrame = Time.frameCount;
            int unityFps = Mathf.RoundToInt((unityFrame - lastUnityFrame) / updateDuration);
            float allocatedMemory = UnityProfiler.GetTotalAllocatedMemoryLong() / BYTES_PER_MB;
            float reservedMemory = UnityProfiler.GetTotalReservedMemoryLong() / BYTES_PER_MB;
            float monoUsedMemory = UnityProfiler.GetMonoUsedSizeLong() / BYTES_PER_MB;
            float monoHeapMemory = UnityProfiler.GetMonoHeapSizeLong() / BYTES_PER_MB;

            text.text =
                $"Logic FPS: {logicFps}\n" +
                $"Unity FPS: {unityFps}\n" +
                $"Unity Frame: {unityFrame}\n" +
                $"Memory Used/Reserved: {allocatedMemory:F1} / {reservedMemory:F1} MB\n" +
                $"Mono Used/Heap: {monoUsedMemory:F1} / {monoHeapMemory:F1} MB\n" +
                $"GC Gen0/1/2: {GC.CollectionCount(0)} / {GC.CollectionCount(1)} / {GC.CollectionCount(2)}";

            lastUpdateTime = now;
            lastUnityFrame = unityFrame;
        }
    }
}
