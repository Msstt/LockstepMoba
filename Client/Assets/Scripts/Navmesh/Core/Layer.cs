using System.Collections.Generic;
using System.Linq;

namespace Navmesh {
    public class Layer {
        
        private NavmeshSurface data;

        private Connection connection;
        
        public Raycaster raycaster;

        public Layer(NavmeshSurface data) {
            this.data = data;
            connection = new Connection(data, this);
            raycaster = new Raycaster(data);
        }

        public bool Init() {
            if (!CheckData()) return false;
            if (!connection.Init()) return false;
            if (!raycaster.Init()) return false;
            return true;
        }

        private bool CheckData() {
            if (data.indices.Count % 3 != 0) {
                Log.Error("NavmeshSurface indices count error");
                return false;
            }
            return true;
        }

        public bool FindPath(Vector3F start, Vector3F end, out List<Vector3F> path) {
            path = new List<Vector3F>();
            if (!raycaster.Raycast(start, out int startTId)) return false;
            if (!raycaster.Raycast(end, out int endTId)) return false;
            
            if (!connection.GetPath(startTId, endTId, out List<Connection.Info> connectionList)) return false;

            int lastTId = startTId;
            for (int i = 0; i < connectionList.Count; i++) {
                DebugUtils.DrawLine(GetCentroid(lastTId), GetCentroid(connectionList[i].tId));
                lastTId = connectionList[i].tId;
            }
            
            return true;
        }
        
        public void DrawTriangle(int tId) {
            int vId1 = data.indices[tId * 3];
            int vId2 = data.indices[tId * 3 + 1];
            int vId3 = data.indices[tId * 3 + 2];
            DebugUtils.DrawLine(data.vertices[vId1], data.vertices[vId2]);
            DebugUtils.DrawLine(data.vertices[vId2], data.vertices[vId3]);
            DebugUtils.DrawLine(data.vertices[vId3], data.vertices[vId1]);
        }
        
        public Vector3F GetCentroid(int tId) {
            var v1 = data.vertices[data.indices[tId * 3]];
            var v2 = data.vertices[data.indices[tId * 3 + 1]];
            var v3 = data.vertices[data.indices[tId * 3 + 2]];
            return (v1 + v2 + v3) / 3;
        }
    }
}