using System;
using System.Collections.Generic;

namespace Navmesh {
    public partial class Raycaster {
        // 返回 start 到 end 之前，最远的在平面内的点
        public Vector3F RaycastInSurface(Vector3F start, Vector3F end) {
            if (GetTIdByPoint(end, false, out int _)) {
                return end;
            }
            var (row1, col1) = GetAABBIndex(start);
            var (row2, col2) = GetAABBIndex(end);
            List<(int, int)> AABBIndex = new List<(int, int)>();
            if (row1 > row2) (row1, row2) = (row2, row1);
            if (col1 > col2) (col1, col2) = (col2, col1);
            for (int i = row1; i <= row2; i++) {
                for (int j = col1; j <= col2; j++) {
                    AABBIndex.Add((i, j));
                }
            }

            Vector3F ret = start;
            void Handle(Vector3F a, Vector3F b) {
                if (GeoUtils.LineIntersect(start, end, a, b, out Vector3F intersect)) {
                    if (Vector3F.Distance2(intersect, end) < Vector3F.Distance2(ret, end)) {
                        ret = intersect;
                    }
                }
            }
            
            foreach (var (row, col) in AABBIndex) {
                foreach (var triId in aabbGrids[row][col]) {
                    Vector3F t1 = data.vertices[data.indices[triId * 3]];
                    Vector3F t2 = data.vertices[data.indices[triId * 3 + 1]];
                    Vector3F t3 = data.vertices[data.indices[triId * 3 + 2]];
                    Handle(t1, t2);
                    Handle(t2, t3);
                    Handle(t3, t1);
                }
            }

            return ret;
        }
    }
}