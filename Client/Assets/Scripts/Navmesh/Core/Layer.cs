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

            if (NavmeshUtils.Config.DrawDebugTriangle) {
                for (int i = 0; i < data.indices.Count / 3; i++) {
                    DrawTriangle(i, 0);
                }
            }
            return true;
        }

        private bool CheckData() {
            if (data.indices.Count == 0 || data.vertices.Count == 0) {
                Log.Error("NavmeshSurface data is empty");
                return false;
            }
            if (data.indices.Count % 3 != 0) {
                Log.Error("NavmeshSurface indices count error");
                return false;
            }
            for (int i = 0; i < data.indices.Count; i++) {
                if (data.indices[i] < 0 || data.indices[i] >= data.vertices.Count) {
                    Log.Error("NavmeshSurface indices has invalid vertex index");
                    return false;
                }
            }
            return true;
        }

        public List<Vector3F> FindPath(Vector3F start, Vector3F end) {
            List<Vector3F> path = new List<Vector3F> { start };
            
            if (!raycaster.GetTIdByPoint(start, true, out int startTId)) return path;
            if (!raycaster.GetTIdByPoint(end, false, out int endTId)) endTId = -1;

            List<Connection.Info> connectionList = connection.GetPath(start, end, startTId, endTId);

            path = smoother.SmoothPath(start, end, startTId, endTId, connectionList);

            for (int i = path.Count - 1; i >= 1; i--) {
                if (Vector3F.IsEqualInEps(path[i - 1] ,path[i], FloatF.eps)) {
                    path.RemoveAt(i);
                }
            }
            return path;
        }
        
        public void DrawTriangle(int tId, float duration = 2f, Color color = default) {
            if (color == default) color = Color.black;
            int vId1 = data.indices[tId * 3];
            int vId2 = data.indices[tId * 3 + 1];
            int vId3 = data.indices[tId * 3 + 2];
            DebugUtils.DrawLine(data.vertices[vId1], data.vertices[vId2], color, duration, 0.8f);
            DebugUtils.DrawLine(data.vertices[vId2], data.vertices[vId3], color, duration, 0.8f);
            DebugUtils.DrawLine(data.vertices[vId3], data.vertices[vId1], color, duration, 0.8f);
        }
        
        public Vector3F GetCentroid(int tId) {
            var v1 = data.vertices[data.indices[tId * 3]];
            var v2 = data.vertices[data.indices[tId * 3 + 1]];
            var v3 = data.vertices[data.indices[tId * 3 + 2]];
            return (v1 + v2 + v3) / 3;
        }
    }
}