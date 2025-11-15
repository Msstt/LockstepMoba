using System.Collections.Generic;
using Framework;
using UnityEngine;
using UnityEngine.AI;

namespace Navmesh {
    public class NavmeshMgr : Singleton<NavmeshMgr> {
        private NavmeshMapInfo mapInfo;
        private Dictionary<FloatF, NavmeshSurface> surfaces;
        
        public void Start() {
            if (!LoadData()) {
                return;
            }

            InitConnection();
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

        // 初始化邻接表
        private void InitConnection() {
        }
    }
}