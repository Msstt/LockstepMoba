using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Navmesh {
    public class NavmeshMgr : Singleton<NavmeshMgr> {
        private NavmeshMapInfo mapInfo;
        private List<FloatF> allRadius;
        private Dictionary<FloatF, NavmeshSurface> surfaces;
        
        private Dictionary<FloatF, Layer> layers;

        public NavmeshMapInfo MapInfo => mapInfo;

        private struct FindPathQuery {
            public FloatF radius;
            public Vector3F start;
            public Vector3F end;
            public Action<List<Vector3F>> callback;
        }
        private Queue<FindPathQuery> findPathQueue = new Queue<FindPathQuery>();

        public void Start() {
            if (!LoadData()) {
                return;
            }

            layers = new Dictionary<FloatF, Layer>();
            allRadius = new List<FloatF>();
            foreach (var (radius, data) in surfaces) {
                var layer = new Layer(data);
                if (!layer.Init()) {
                    return;
                }

                allRadius.Add(radius);
                layers.Add(radius, layer);
            }
            allRadius.Sort((a, b) => b.CompareTo(a) ); 
        }

        private bool LoadData() {
            mapInfo = GameObject.Find("Map")?.GetComponent<NavmeshMapInfo>();
            if (mapInfo == null) {
                Log.Error("NavmeshMapInfo not found");
                return false;
            }
            if (mapInfo.surfaceData == null) {
                Log.Error("Navmesh SurfaceData not found");
                return false;
            }
            if (!JsonHelper.LoadFromString(mapInfo.surfaceData.text, out surfaces)) {
                Log.Error("Navmesh SurfaceData parse failed");
                return false;
            }
            return true;
        }
        
        public void Update() {
            HandleFindPathQueue();
        }

        #region 寻路

        private List<Vector3F> FindPath(FloatF radius, Vector3F start, Vector3F end) {
            foreach (var r in allRadius) {
                if (radius <= r) {
                    return layers[r].FindPath(start, end);
                }
            }
            return new List<Vector3F> { start };
        }
        
        public void FindPath(FloatF radius, Vector3F start, Vector3F end, Action<List<Vector3F>> callback, bool force) {
            if (force) {
                var path = FindPath(radius, start, end);
                callback?.Invoke(path);
                return;
            }
            findPathQueue.Enqueue(new FindPathQuery {
                radius = radius,
                start = start,
                end = end,
                callback = callback,
            });
        }

        private void HandleFindPathQueue() {
            for (int i = 0; i < FindPathConfig.FindPathMaxQueryPerFrame; i++) {
                if (findPathQueue.Count == 0) {
                    break;
                }
                var query = findPathQueue.Dequeue();
                var path = FindPath(query.radius, query.start, query.end);
                query.callback?.Invoke(path);
            }
        }

        #endregion

        #region 射线检测
        
        public bool Raycast(FloatF radius, Vector3F point, out int tId) {
            tId = -1;
            foreach (var r in allRadius) {
                if (radius <= r) {
                    return layers[r].raycaster.Raycast(point, out tId);
                }
            }
            return false;
        }

        #endregion
    }
}