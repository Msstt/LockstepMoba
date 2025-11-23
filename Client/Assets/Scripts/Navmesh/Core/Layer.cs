using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Navmesh {
    public class Layer {
        
        private NavmeshSurface data;

        private Connection connection;
        private PathSmoother smoother;
        
        public Raycaster raycaster;

        public Layer(NavmeshSurface data) {
            this.data = data;
            connection = new Connection(data, this);
            smoother = new PathSmoother(data);
            raycaster = new Raycaster(data);
        }

        public bool Init() {
            if (!CheckData()) return false;
            if (!connection.Init()) return false;
            if (!raycaster.Init()) return false;

            for (int i = 0; i < data.indices.Count / 3; i++) {
                DrawTriangle(i, 0);
            }
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
            
            if (!connection.GetPath(start, end, startTId, endTId, out List<Connection.Info> connectionList)) return false;
            
            int lastTId = startTId;
            for (int i = 0; i < connectionList.Count; i++) {
                DebugUtils.DrawLine(GetCentroid(lastTId), GetCentroid(connectionList[i].tId), Color.blue);
                lastTId = connectionList[i].tId;
            }

            path = smoother.SmoothPath(start, end, startTId, connectionList);
            for (int i = 0; i + 1 < path.Count; i++) {
                DebugUtils.DrawLine(path[i], path[i + 1]);
            }
            return true;
        }
        
        public void DrawTriangle(int tId, float duration = 2f) {
            int vId1 = data.indices[tId * 3];
            int vId2 = data.indices[tId * 3 + 1];
            int vId3 = data.indices[tId * 3 + 2];
            DebugUtils.DrawLine(data.vertices[vId1], data.vertices[vId2], Color.black, duration, 0.03f);
            DebugUtils.DrawLine(data.vertices[vId2], data.vertices[vId3], Color.black, duration, 0.03f);
            DebugUtils.DrawLine(data.vertices[vId3], data.vertices[vId1], Color.black, duration, 0.03f);
        }
        
        public Vector3F GetCentroid(int tId) {
            var v1 = data.vertices[data.indices[tId * 3]];
            var v2 = data.vertices[data.indices[tId * 3 + 1]];
            var v3 = data.vertices[data.indices[tId * 3 + 2]];
            return (v1 + v2 + v3) / 3;
        }
    }
}