// 漏斗算法平滑路径

using System.Collections.Generic;
using UnityEngine;

namespace Navmesh {
    public class PathSmoother {
        private NavmeshSurface data;
        
        public PathSmoother(NavmeshSurface data) {
            this.data = data;
        }

        public List<Vector3F> SmoothPath(Vector3F start, Vector3F end, int startId, List<Connection.Info> connection) {
            if (connection.Count == 0) {
                return new List<Vector3F> { start, end };
            }
            
            List<Vector3F> path = new List<Vector3F> { start };
            
            Vector3F curPoint = start, left = curPoint, right = curPoint;

            bool IsRight(Vector3F v1, Vector3F v2) {
                return Vector3F.Cross(v2 - curPoint, v1 - curPoint).y >= 0;
            }

            int curLeftIndex = -1, curRightIndex = -1;
            for (int i = 0; i <= connection.Count; i++) {
                if (i < 0) {
                    Log.Error("PathSmoother SmoothPath i < 0");
                    break;
                }
                
                Vector3F newLeft, newRight;
                if (i < connection.Count) {
                    newLeft = data.vertices[connection[i].vId1];
                    newRight = data.vertices[connection[i].vId2];
                } else {
                    newLeft = end;
                    newRight = end;
                }

                if (IsRight(left, newLeft) && IsRight(newLeft, right)) {
                    left = newLeft;
                    curLeftIndex = i;
                }
                
                if (IsRight(left, newRight) && IsRight(newRight, right)) {
                    right = newRight;
                    curRightIndex = i;
                }

                if (!IsRight(newLeft, right)) {
                    curPoint = right;
                    i = curRightIndex;
                } else if (!IsRight(left, newRight)) {
                    curPoint = left;
                    i = curLeftIndex;
                } else {
                    continue;
                }
                path.Add(curPoint);
                left = curPoint;
                right = curPoint;
                curLeftIndex = -1;
                curRightIndex = -1;
            }
            
            path.Add(end);
            return path;
        }
    }
}