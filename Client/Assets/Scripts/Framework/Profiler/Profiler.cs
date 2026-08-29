using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Profiling;

namespace Framework {
    public sealed class Profiler : Singleton<Profiler> {
        private readonly ProfilerMarker updateMarker = new ProfilerMarker("GameMgr.Update");
        private readonly ProfilerMarker frameUpdateMarker = new ProfilerMarker("GameMgr.FrameUpdate");
        private readonly Dictionary<Type, ProfilerMarker> updateSystemMarkers = new Dictionary<Type, ProfilerMarker>();
        private readonly Dictionary<Type, ProfilerMarker> frameUpdateSystemMarkers = new Dictionary<Type, ProfilerMarker>();
        private readonly Dictionary<Type, ProfilerMarker> actorComUpdateMarkers = new Dictionary<Type, ProfilerMarker>();
        private readonly Dictionary<Type, ProfilerMarker> areaEffectUpdateMarkers = new Dictionary<Type, ProfilerMarker>();

        [Conditional("ENABLE_PROFILER")]
        public void BeginUpdate() {
            updateMarker.Begin();
        }

        [Conditional("ENABLE_PROFILER")]
        public void EndUpdate() {
            updateMarker.End();
        }

        [Conditional("ENABLE_PROFILER")]
        public void BeginFrameUpdate() {
            frameUpdateMarker.Begin();
        }

        [Conditional("ENABLE_PROFILER")]
        public void EndFrameUpdate() {
            frameUpdateMarker.End();
        }

        [Conditional("ENABLE_PROFILER")]
        public void BeginUpdateSystem(Type type) {
            GetMarker(updateSystemMarkers, "GameMgr.Update.", type).Begin();
        }

        [Conditional("ENABLE_PROFILER")]
        public void EndUpdateSystem(Type type) {
            GetMarker(updateSystemMarkers, "GameMgr.Update.", type).End();
        }

        [Conditional("ENABLE_PROFILER")]
        public void BeginFrameUpdateSystem(Type type) {
            GetMarker(frameUpdateSystemMarkers, "GameMgr.FrameUpdate.", type).Begin();
        }

        [Conditional("ENABLE_PROFILER")]
        public void EndFrameUpdateSystem(Type type) {
            GetMarker(frameUpdateSystemMarkers, "GameMgr.FrameUpdate.", type).End();
        }

        [Conditional("ENABLE_PROFILER")]
        public void BeginActorComUpdate(Type type) {
            GetMarker(actorComUpdateMarkers, "Actor.Com.Update.", type).Begin();
        }

        [Conditional("ENABLE_PROFILER")]
        public void EndActorComUpdate(Type type) {
            GetMarker(actorComUpdateMarkers, "Actor.Com.Update.", type).End();
        }

        [Conditional("ENABLE_PROFILER")]
        public void BeginAreaEffectUpdate(Type type) {
            GetMarker(areaEffectUpdateMarkers, "Area.Effect.Update.", type).Begin();
        }

        [Conditional("ENABLE_PROFILER")]
        public void EndAreaEffectUpdate(Type type) {
            GetMarker(areaEffectUpdateMarkers, "Area.Effect.Update.", type).End();
        }

        private static ProfilerMarker GetMarker(Dictionary<Type, ProfilerMarker> markers, string prefix, Type type) {
            if (!markers.TryGetValue(type, out ProfilerMarker marker)) {
                marker = new ProfilerMarker(prefix + type.FullName);
                markers.Add(type, marker);
            }
            return marker;
        }
    }
}
