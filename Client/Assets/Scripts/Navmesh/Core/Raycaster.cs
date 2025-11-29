using System.Collections.Generic;

namespace Navmesh {
    public class Raycaster {
        private static int MaxAABBCount = 10;
        
        private NavmeshSurface data;
        
        private List<List<List<int>>> aabbGrids;

        private Vector3F min, max;
        private Vector3F size;
        
        public Raycaster(NavmeshSurface data) {
            this.data = data;
        }

        public bool Init() {
            min = max = data.vertices[0];
            for (int i = 0; i < data.vertices.Count; i++) {
                min.x = FloatF.Min(min.x, data.vertices[i].x);
                min.y = FloatF.Min(min.y, data.vertices[i].y);
                min.z = FloatF.Min(min.z, data.vertices[i].z);
                max.x = FloatF.Max(max.x, data.vertices[i].x);
                max.y = FloatF.Max(max.y, data.vertices[i].y);
                max.z = FloatF.Max(max.z, data.vertices[i].z);
            }
            size = new Vector3F((max.x - min.x) / MaxAABBCount, (max.y - min.y) / MaxAABBCount, (max.z - min.z) / MaxAABBCount);
            
            aabbGrids = new List<List<List<int>>>();
            for (int row = 0; row < MaxAABBCount; row++) {
                aabbGrids.Add(new List<List<int>>());
                for (int col = 0; col < MaxAABBCount; col++) {
                    aabbGrids[row].Add(new List<int>());
                    InitAABB(row, col);
                }
            }
            
            return true;
        }

        private void InitAABB(int row, int col) {
            for (int i = 0; i < data.indices.Count / 3; i++) {
                if (TriangleInAABB(row, col, i)) {
                    aabbGrids[row][col].Add(i);
                }
            }
        }
        
        bool TriangleInAABB(int row, int col, int tId) {
            Vector3F p1 = min + new Vector3F(row * size.x, 0, col * size.z);
            Vector3F p2 = min + new Vector3F(row * size.x, 0, (col + 1) * size.z);
            Vector3F p3 = min + new Vector3F((row + 1) * size.x, 0, col * size.z);
            Vector3F p4 = min + new Vector3F((row + 1) * size.x, 0, (col + 1) * size.z);

            Vector3F t1 = data.vertices[data.indices[tId * 3]];
            Vector3F t2 = data.vertices[data.indices[tId * 3 + 1]];
            Vector3F t3 = data.vertices[data.indices[tId * 3 + 2]];
            
            if (PointInTriangle(p1, tId) || PointInTriangle(p2, tId) || PointInTriangle(p3, tId) ||
                PointInTriangle(p4, tId)) return true;
            
            bool InAABB(Vector3F point) {
                return point.x >= p1.x && point.x <= p4.x && point.z >= p1.z && point.z <= p4.z;
            }
            
            if (InAABB(t1) || InAABB(t2) || InAABB(t3)) return true;

            bool Intersect(Vector3F a, Vector3F b) {
                return GeoUtils.LineIsIntersectInXZ(a, b, p1, p2) || 
                       GeoUtils.LineIsIntersectInXZ(a, b, p2, p4) ||
                       GeoUtils.LineIsIntersectInXZ(a, b, p4, p3) ||
                       GeoUtils.LineIsIntersectInXZ(a, b, p3, p1);
            }
            
            if (Intersect(t1, t2) || Intersect(t2, t3) || Intersect(t3, t1)) return true;

            return false;
        }
        
        bool PointInTriangle(Vector3F point, int tId) {
            Vector3F vId1 = data.vertices[data.indices[tId * 3]];
            Vector3F vId2 = data.vertices[data.indices[tId * 3 + 1]];
            Vector3F vId3 = data.vertices[data.indices[tId * 3 + 2]];
            FloatF d1 = Vector3F.Cross(point - vId1, vId2 - vId1).y;
            FloatF d2 = Vector3F.Cross(point - vId2, vId3 - vId2).y;
            FloatF d3 = Vector3F.Cross(point - vId3, vId1 - vId3).y;
            bool hasNeg = (d1 < 0) || (d2 < 0) || (d3 < 0);
            bool hasPos = (d1 > 0) || (d2 > 0) || (d3 > 0);
            return !(hasNeg && hasPos);
        }

        // XZ 平面射线检测，返回三角形 id
        public bool Raycast(Vector3F point, out int tId) {
            tId = -1;
            int row = (int)((point - min).x / size.x);
            int col = (int)((point - min).z / size.z);
            if (row < 0 || row >= MaxAABBCount || col < 0 || col >= MaxAABBCount) {
                return false;
            }
            for (int i = 0; i < aabbGrids[row][col].Count; i++) {
                int triId = aabbGrids[row][col][i];
                if (PointInTriangle(point, triId)) {
                    tId = triId;
                    return true;
                }
            }
            for (int i = 0; i < data.indices.Count / 3; i++) {
                if (PointInTriangle(point, i)) {
                    tId = i;
                    return true;
                }
            }
            return false;
        }
    }
}