// 漏斗算法平滑路径

using System.Collections.Generic;
using UnityEngine;

namespace Navmesh {
    public class PathSmoother {
        private NavmeshSurface data;
        
        public PathSmoother(NavmeshSurface data) {
            this.data = data;
        }

        // TODO: 不可达时走向边界
        public List<Vector3F> SmoothPath(Vector3F start, Vector3F end, int startId, int endTId, List<Connection.Info> connection) {
            if (connection.Count == 0) {
                if (endTId != -1) {
                    return new List<Vector3F> { start, end };
                } else {
                    return new List<Vector3F> { start };
                }
            }
            
            List<Vector3F> path = new List<Vector3F> { start };
            
            Vector3F curPoint = start, left = curPoint, right = curPoint;

            bool IsRight(Vector3F v1, Vector3F v2) {
                return Vector3F.Cross(v2 - curPoint, v1 - curPoint).y >= 0;
            }
            
            bool reachEnd = endTId == connection[^1].tId;

            int curLeftIndex = -1, curRightIndex = -1;
            for (int i = 0; i < connection.Count + (reachEnd ? 1 : 0); i++) {
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

            if (reachEnd) {
                path.Add(end);
            } else {
                path.Add(Vector3F.Mid(left, right));
            }
            return path;
        }
    }
}