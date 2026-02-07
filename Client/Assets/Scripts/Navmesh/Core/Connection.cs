using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Navmesh {
    public class Connection {
        public class Info {
            public FloatF centroidDis;
            public int vId1;
            public int vId2;
            public int tId;
        }
        
        private NavmeshSurface data;
        private Layer layer;
        
        private List<SortedDictionary<int, Info>> connections;
        private Dictionary<(int, int, int), FloatF> midDis;
        private List<Vector3F> centroid;
        
        public Connection(NavmeshSurface data, Layer layer) {
            this.data = data;
            this.layer = layer;
        }
        
        public bool Init() {
            Dictionary<(int, int), int> edges = new Dictionary<(int, int), int>();
            void AddEdge(int index, int x, int y) {
                edges.TryAdd((x, y), index);
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
            
            connections = new List<SortedDictionary<int, Info>>();
            centroid = new List<Vector3F>();
            for (int i = 0; i < data.indices.Count / 3; i++) {
                connections.Add(new SortedDictionary<int, Info>());
                centroid.Add(layer.GetCentroid(i));
            }
            foreach (var ((vId1, vId2), sTId) in edges) {
                if (edges.TryGetValue((vId2, vId1), out int tTId)) {
                    connections[sTId].TryAdd(tTId, new Info {
                        centroidDis = Vector3F.Distance(centroid[sTId], centroid[tTId]),
                        tId = tTId,
                        vId1 = vId1,
                        vId2 = vId2,
                    });

                    if (NavmeshUtils.Config.DrawDebugConnection && FindPathConfig.wType == FindPathConfig.WType.centroidDis) {
                        DebugUtils.DrawLine(centroid[sTId], centroid[tTId], Color.green, 0);
                    }
                }
            }

            midDis = new Dictionary<(int, int, int), FloatF>();
            for (int sTId = 0; sTId < data.indices.Count / 3; sTId++) {
                foreach (var pTId in connections[sTId].Keys) {
                    foreach (var tTId in connections[sTId].Keys) {
                        if (pTId == tTId) {
                            continue;
                        }
                        Vector3F p1 = GetEdgeMidPoint(connections[pTId][sTId]);
                        Vector3F p2 = GetEdgeMidPoint(connections[sTId][tTId]);
                        midDis[(pTId, sTId, tTId)] = Vector3F.Distance(p1, p2);
                        
                        if (NavmeshUtils.Config.DrawDebugConnection && FindPathConfig.wType == FindPathConfig.WType.edgeMidDis) {
                            DebugUtils.DrawLine(p1, p2, Color.green, 0);
                        }
                    }
                }
            }

            return true;
        }
        
        private Vector3F GetEdgeMidPoint(Info info) {
            return Vector3F.Mid(data.vertices[info.vId1], data.vertices[info.vId2]);
        }

        // 获取最短路
        // startId, endId: 三角形ID
        public List<Info> GetPath(Vector3F start, Vector3F end, int startId, int endId) {
            List<Info> path = new List<Info>();
            
            Dictionary<int, FloatF> cost = new Dictionary<int, FloatF>();
            Dictionary<int, int> parent = new Dictionary<int, int>();
            HashSet<int> visited = new HashSet<int>();
            PriorityQueue<int, FloatF> queue = new PriorityQueue<int, FloatF>();
            queue.Enqueue(startId, 0);
            cost[startId] = 0;
            parent[startId] = -1;
            
            FloatF GetW(int pTId, int sTId, int tTId) {
                switch (FindPathConfig.wType) {
                    case FindPathConfig.WType.centroidDis:
                        if (sTId == startId && tTId == endId) {
                            return Vector3F.Distance(start, end);
                        } else if (sTId == startId) {
                            return Vector3F.Distance(start, centroid[tTId]);
                        } else if (tTId == endId) {
                            return Vector3F.Distance(centroid[sTId], end);
                        } else {
                            return connections[sTId][tTId].centroidDis;
                        }
                    case FindPathConfig.WType.edgeMidDis:
                        if (sTId == startId && tTId == endId) {
                            return Vector3F.Distance(start, end);
                        } else if (sTId == startId) {
                            Info info = connections[sTId][tTId];
                            return Vector3F.Distance(start, Vector3F.Mid(data.vertices[info.vId1], data.vertices[info.vId2]));
                        } else if (tTId == endId) {
                            Info info = connections[pTId][sTId];
                            return Vector3F.Distance(Vector3F.Mid(data.vertices[info.vId1], data.vertices[info.vId2]), end);
                        } else {
                            return midDis[(pTId, sTId, tTId)];
                        }
                }
                return FloatF.zero;
            }
            
            FloatF GetH(int id) {
                return Vector3F.MaxDistance(centroid[id], end);
            }
            
            FloatF GetF(int id) {
                // return cost[id];
                return cost[id] + GetH(id); // f = g + h
                // return cost[id] + Vector3F.Distance2(centroid[id], end); // f = g + h
            }

            int nearestId = startId;
            FloatF nearestDis = GetH(startId);
            while (queue.Count != 0) {
                queue.Dequeue(out int currentId, out _);
                
                if (currentId == endId || visited.Count > FindPathConfig.FindPathMaxIterationCount) {
                    break;
                }

                if (visited.Contains(currentId)) {
                    continue;
                }
                visited.Add(currentId);

                foreach (var info in connections[currentId].Values) {
                    if (parent[currentId] == info.tId) {
                        continue;
                    }
                    FloatF newCost = cost[currentId] + GetW(parent[currentId], currentId, info.tId);
                    if (!cost.ContainsKey(info.tId) || newCost < cost[info.tId]) {
                        cost[info.tId] = newCost;
                        parent[info.tId] = currentId;
                        queue.Enqueue(info.tId, GetF(info.tId));
                        
                        FloatF h = GetH(info.tId);
                        if (info.tId == endId) {
                            h = FloatF.zero;
                        }
                        if (h < nearestDis) {
                            nearestId = info.tId;
                            nearestDis = h;
                        }
                    }
                }
            }
            
            while (nearestId != startId) {
                int pId = parent[nearestId];
                path.Add(connections[pId][nearestId]);
                nearestId = pId;
            }
            path.Reverse();
            return path;
        }
    }
}