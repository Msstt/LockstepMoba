using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Navmesh {
    public class NavmeshMgr : Singleton<NavmeshMgr> {
        private NavmeshMapInfo mapInfo;
        private Dictionary<FloatF, NavmeshSurface> surfaces;
        
        private Dictionary<FloatF, Layer> layers;
        
        public void Start() {
            if (!LoadData()) {
                return;
            }

            layers = new Dictionary<FloatF, Layer>();
            foreach (var (radius, data) in surfaces) {
                var layer = new Layer(data);
                if (!layer.Init()) {
                    return;
                }
                layers.Add(radius, layer);
            }
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
    }
}