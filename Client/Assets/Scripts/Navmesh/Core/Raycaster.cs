using System.Collections.Generic;

namespace Navmesh {
    public class Raycaster {
        private static int MaxAABBCount = 10;
        private static int AABBSize = 2;
        
        private NavmeshSurface data;
        
        private List<List<List<int>>> aabbGrids;
        
        public Raycaster(NavmeshSurface data) {
            this.data = data;
        }

        public bool Init() {
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
            Vector3F p1 = new Vector3F(row * AABBSize, 0, col * AABBSize);
            Vector3F p2 = new Vector3F(row * AABBSize, 0, (col + 1) * AABBSize);
            Vector3F p3 = new Vector3F((row + 1) * AABBSize, 0, col * AABBSize);
            Vector3F p4 = new Vector3F((row + 1) * AABBSize, 0, (col + 1) * AABBSize);

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
            int vId1 = data.indices[tId * 3];
            int vId2 = data.indices[tId * 3 + 1];
            int vId3 = data.indices[tId * 3 + 2];
            FloatF Sign(Vector3F p1, Vector3F p2, Vector3F p3) =>
                (p1.x - p3.x) * (p2.z - p3.z) - (p2.x - p3.x) * (p1.z - p3.z);
            FloatF d1 = Sign(point, data.vertices[vId1], data.vertices[vId2]);
            FloatF d2 = Sign(point, data.vertices[vId2], data.vertices[vId3]);
            FloatF d3 = Sign(point, data.vertices[vId3], data.vertices[vId1]);
            bool hasNeg = (d1 < 0) || (d2 < 0) || (d3 < 0);
            bool hasPos = (d1 > 0) || (d2 > 0) || (d3 > 0);
            return !(hasNeg && hasPos);
        }

        // XZ 平面射线检测，返回三角形 id
        public bool Raycast(Vector3F point, out int tId) {
            tId = -1;
            int row = (int)(point.x / AABBSize);
            int col = (int)(point.z / AABBSize);
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