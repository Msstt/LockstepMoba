using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Navmesh {
    public class Navmesh : INavmesh {
        private NavmeshMapInfo mapInfo;
        private List<FloatF> allRadius;
        private Dictionary<FloatF, NavmeshSurface> surfaces;
        
        private Dictionary<FloatF, Layer> layers;

        public NavmeshMapInfo MapInfo => mapInfo;

        private UnitRaycaster unitRaycaster = new UnitRaycaster();

        private struct FindPathQuery {
            public FloatF radius;
            public Vector3F start;
            public Vector3F end;
            public Action<List<Vector3F>> callback;
        }
        private Queue<FindPathQuery> findPathQueue = new Queue<FindPathQuery>();

        public void Init() {
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
            
            var (min, max) = surfaces[allRadius[0]].GetBorder();
            unitRaycaster.Init(min, max);
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
        
        public void FrameUpdate(int frame) {
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
        
        public bool IsReachable(FloatF radius, Vector3F point) {
            foreach (var r in allRadius) {
                if (radius <= r) {
                    return layers[r].raycaster.GetTIdByPoint(point, false, out _);
                }
            }
            return false;
        }
        
        public Vector3F RaycastInSurface(FloatF radius, Vector3F start, Vector3F end) {
            foreach (var r in allRadius) {
                if (radius <= r) {
                    return layers[r].raycaster.RaycastInSurface(start, end);
                }
            }
            return start;
        }

        #endregion

        #region 单位检测

        public void RegisterUnit(int id, int type, Vector3F pos, SafeEvent<Vector3F> onPosChange) {
            unitRaycaster.Register(id, type, pos, onPosChange);
        }
        
        public void UnRegisterUnit(int id, SafeEvent<Vector3F> onPosChange) {
            unitRaycaster.UnRegister(id, onPosChange);
        }

        public List<int> RaycastInCircle(int typeBitSet, Vector3F center, FloatF radius) {
            return unitRaycaster.RaycastInCircle(typeBitSet, center, radius);
        }

        #endregion
    }
}