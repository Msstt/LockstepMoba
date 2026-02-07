using System;
using System.Collections.Generic;

namespace Navmesh {
    public partial class Raycaster {
        // XZ 平面射线检测，返回三角形 id
        public bool GetTIdByPoint(Vector3F point, bool findNearest, out int tId) {
            tId = -1;
            var (row, col) = GetAABBIndex(point);
            List<(int, int)> AABBIndex = new List<(int, int)>();
            AABBIndex.Add((row, col));
            // 三角形内
            foreach (var (r, c) in AABBIndex) {
                if (r < 0 || r >= MaxAABBCount || c < 0 || c >= MaxAABBCount) {
                    continue;
                }
                foreach (var triId in aabbGrids[r][c]) {
                    if (PointInTriangle(point, triId)) {
                        tId = triId;
                        return true;
                    }
                }
            }
            if (findNearest) {
                AABBIndex.Add((row - 1, col - 1));
                AABBIndex.Add((row - 1, col));
                AABBIndex.Add((row, col - 1));
                AABBIndex.Add((row, col + 1));
                AABBIndex.Add((row + 1, col));
                AABBIndex.Add((row + 1, col + 1));
                AABBIndex.Add((row - 1, col + 1));
                AABBIndex.Add((row + 1, col - 1));
                // 最近三角形
                FloatF minDis = FloatF.max;
                foreach (var (r, c) in AABBIndex) {
                    if (r < 0 || r >= MaxAABBCount || c < 0 || c >= MaxAABBCount) {
                        continue;
                    }
                    foreach (var triId in aabbGrids[r][c]) {
                        FloatF dis = PointToTriangleDistance(point, triId);
                        if (dis < minDis) {
                            tId = triId;
                            minDis = dis;
                        }
                    }
                }
                return tId != -1;
            }
            
            // for (int i = 0; i < data.indices.Count / 3; i++) {
            //     if (PointInTriangle(point, i)) {
            //         tId = i;
            //         return true;
            //     }
            // }
            return false;
        }
    }
}