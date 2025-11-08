using Framework;
using UnityEngine;
using UnityEngine.AI;

namespace Navmesh {
    public class NavmeshMgr : Singleton<NavmeshMgr> {
        private NavmeshMapInfo mapInfo;
        
        public void Start() {
            mapInfo = GameObject.Find("Map/Terrain")?.GetComponent<NavmeshMapInfo>();
            if (mapInfo == null) {
                Log.Error("NavmeshMapInfo not found");
                return;
            }

            InitConnection();
        }

        // 初始化邻接表
        private void InitConnection() {
            if (mapInfo.data == null) {
                Log.Error("NavmeshData not found");
                return;
            }
            
            var tri = NavMesh.CalculateTriangulation();
        }
    }
}