// 漏斗算法平滑路径

using System.Collections.Generic;

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

            int curIndex = -1;
            for (int i = 0; i < connection.Count; i++) {
                if (i < 0) {
                    Log.Error("PathSmoother SmoothPath i < 0");
                    break;
                }
                
                Vector3F newLeft = data.vertices[connection[i].vId1], newRight = data.vertices[connection[i].vId2];

                if (IsRight(left, newLeft) && IsRight(newLeft, right) && IsRight(left, newRight) &&
                    IsRight(newRight, right)) {
                    left = newLeft;
                    right = newRight;
                    curIndex = i;
                    continue;
                }

                if (IsRight(right, newLeft)) {
                    curPoint = right;
                } else if (IsRight(newRight, left)) {
                    curPoint = left;
                } else {
                    continue;
                }
                path.Add(curPoint);
                left = curPoint;
                right = curPoint;
                i = curIndex - 1;
                curIndex = i;
            }

            if (IsRight(end, left)) {
                path.Add(left);
            } else if (IsRight(right, end)) {
                path.Add(right);
            }
            path.Add(end);
            return path;
        }
    }
}