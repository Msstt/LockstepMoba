using System;
using System.Collections.Generic;

namespace Navmesh {
    public class Connection {
        public class Info {
            public FloatF w;
            public int vId1;
            public int vId2;
            public int tId;
        }
        
        private NavmeshSurface data;
        
        private List<Dictionary<int, Info>> connections;
        
        public Connection(NavmeshSurface data) {
            this.data = data;
        }
        
        public bool Init() {
            Dictionary<Tuple<int, int>, int> edges = new Dictionary<Tuple<int, int>, int>();
            void AddEdge(int index, int x, int y) {
                var edge = Tuple.Create(x, y);
                edges.TryAdd(edge, index);
            }
            for (int i = 0; i < data.indices.Count; i += 3) {
                AddEdge(i / 3, data.indices[i], data.indices[i + 1]);
                AddEdge(i / 3, data.indices[i + 1], data.indices[i + 2]);
                AddEdge(i / 3, data.indices[i + 2], data.indices[i]);
            }

            if (edges.Count != data.indices.Count) {
                Log.Error("NavmeshSurface indices has multiple triangles share the same edge");
                return false;
            }
            
            connections = new List<Dictionary<int, Info>>(data.indices.Count / 3);
            foreach (var ((vId1, vId2), tId1) in edges) {
                var revEdge = Tuple.Create(vId2, vId1);
                if (edges.TryGetValue(revEdge, out int tId2)) {
                    connections[tId1].Add(tId2, new Info {
                        w = GetW(tId1, tId2),
                        tId = tId2,
                        vId1 = vId1,
                        vId2 = vId2,
                    });
                }
            }

            return true;
        }

        private Vector3F GetCentroid(int tId) {
            var v1 = data.vertices[data.indices[tId * 3]];
            var v2 = data.vertices[data.indices[tId * 3 + 1]];
            var v3 = data.vertices[data.indices[tId * 3 + 2]];
            return (v1 + v2 + v3) / 3;
        }

        private FloatF GetW(int tId1, int tId2) {
            return Vector3F.DistanceF(GetCentroid(tId1), GetCentroid(tId2));
        }

        // // 获取最短路
        // public List<Info> GetPath(int startId, int endId) {
        //     HashSet<int> visited = new HashSet<int>();
        //     PriorityQueue<int, FloatF> queue = new PriorityQueue<int, FloatF>();
        //     queue.Enqueue(startId, 0);
        // }

        private void GetH(int tId) {
        }
    }
}