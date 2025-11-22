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
            mapInfo = GameObject.Find("Map/Terrain")?.GetComponent<NavmeshMapInfo>();
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

        #region 寻路

        public bool FindPath(FloatF radius, Vector3F start, Vector3F end, out List<Vector3F> path) {
            path = new List<Vector3F>();
            foreach (var r in allRadius) {
                if (radius <= r) {
                    return layers[r].FindPath(start, end, out path);
                }
            }
            return false;
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