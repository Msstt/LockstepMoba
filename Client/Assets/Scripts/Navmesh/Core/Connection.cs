using System;
using System.Collections.Generic;
using Framework;

namespace Navmesh {
    public class Connection {
        public class Info {
            public FloatF w;
            public int vId1;
            public int vId2;
            public int tId;
        }
        
        private NavmeshSurface data;
        private Layer layer;
        
        private List<Dictionary<int, Info>> connections;
        private List<Vector3F> centroid;
        
        public Connection(NavmeshSurface data, Layer layer) {
            this.data = data;
            this.layer = layer;
        }
        
        public bool Init() {
            Dictionary<Tuple<int, int>, int> edges = new Dictionary<Tuple<int, int>, int>();
            void AddEdge(int index, int x, int y) {
                var edge = Tuple.Create(x, y);
                edges.TryAdd(edge, index);
            }
            for (int i = 0; i < data.indices.Count; i += 3) {
                int p1 = data.indices[i], p2 = data.indices[i + 1], p3 = data.indices[i + 2];
                if (Vector3F.Cross(data.vertices[p2] - data.vertices[p1], data.vertices[p3] - data.vertices[p1]).y > 0) {
                    (p3, p1) = (p1, p3);
                }
                AddEdge(i / 3, p1, p2);
                AddEdge(i / 3, p2, p3);
                AddEdge(i / 3, p3, p1);
            }

            if (edges.Count != data.indices.Count) {
                Log.Error("NavmeshSurface indices has multiple triangles share the same edge");
                return false;
            }
            
            connections = new List<Dictionary<int, Info>>();
            centroid = new List<Vector3F>();
            for (int i = 0; i < data.indices.Count / 3; i++) {
                connections.Add(new Dictionary<int, Info>());
                centroid.Add(layer.GetCentroid(i));
            }
            foreach (var ((vId1, vId2), tId1) in edges) {
                var revEdge = Tuple.Create(vId2, vId1);
                if (edges.TryGetValue(revEdge, out int tId2)) {
                    connections[tId1].TryAdd(tId2, new Info {
                        w = GetW(tId1, tId2, vId1, vId2),
                        tId = tId2,
                        vId1 = vId1,
                        vId2 = vId2,
                    });
                }
            }

            return true;
        }

        private FloatF GetW(int tId1, int tId2, int vId1, int vId2) {
            return Vector3F.Distance(centroid[tId1], centroid[tId2]);
            // return Vector3F.Distance(centroid[tId1], Vector3F.Mid(data.vertices[vId1], data.vertices[vId2]));\
        }

        // 获取最短路
        // startId, endId: 三角形ID
        public bool GetPath(Vector3F start, Vector3F end, int startId, int endId, out List<Info> path) {
            path = new List<Info>();
            
            Dictionary<int, FloatF> cost = new Dictionary<int, FloatF>();
            Dictionary<int, int> parent = new Dictionary<int, int>();
            HashSet<int> visited = new HashSet<int>();
            PriorityQueue<int, FloatF> queue = new PriorityQueue<int, FloatF>();
            queue.Enqueue(startId, 0);
            cost[startId] = 0;

            FloatF GetF(int id) {
                // return cost[id];
                return cost[id] + Vector3F.MaxDistance(centroid[id], centroid[endId]);; // f = g + h
                // return cost[id] + Vector3F.Distance2(centroid[id], centroid[endId]);; // f = g + h
            }

            while (queue.Count != 0) {
                queue.Dequeue(out int currentId, out _);
                
                if (currentId == endId) {
                    while (currentId != startId) {
                        int pId = parent[currentId];
                        path.Add(connections[pId][currentId]);
                        currentId = pId;
                    }
                    path.Reverse();
                    return true;
                }

                if (visited.Contains(currentId)) {
                    continue;
                }
                visited.Add(currentId);

                foreach (var info in connections[currentId].Values) {
                    FloatF newCost = cost[currentId] + info.w;
                    if (currentId == startId) {
                        newCost = cost[currentId] + Vector3F.Distance(start, centroid[info.tId]);
                    }
                    if (info.tId == endId) {
                        newCost = cost[currentId] + Vector3F.Distance(centroid[info.tId], end);
                    }
                    if (!cost.ContainsKey(info.tId) || newCost < cost[info.tId]) {
                        cost[info.tId] = newCost;
                        parent[info.tId] = currentId;
                        queue.Enqueue(info.tId, GetF(info.tId));
                    }
                }
            }

            return false;
        }
    }
}