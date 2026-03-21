using System.Collections.Generic;

namespace Navmesh {
    public partial class Raycaster {
        private static int MaxAABBCount = 10;
        
        private NavmeshSurface data;
        
        private List<int>[][] aabbGrids;

        private Vector3F min, max;
        private Vector3F size;
        
        public Raycaster(NavmeshSurface data) {
            this.data = data;
        }

        public bool Init() {
            (min, max) = data.GetBorder();
            size = new Vector3F((max.x - min.x) / MaxAABBCount, (max.y - min.y) / MaxAABBCount, (max.z - min.z) / MaxAABBCount);
            
            aabbGrids = new List<int>[MaxAABBCount][];
            for (int row = 0; row < MaxAABBCount; row++) {
                aabbGrids[row] = new List<int>[MaxAABBCount];
                for (int col = 0; col < MaxAABBCount; col++) {
                    aabbGrids[row][col] = new List<int>();
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
        
        private bool TriangleInAABB(int row, int col, int tId) {
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
                return GeoUtils.LineIsIntersect(a, b, p1, p2) || 
                       GeoUtils.LineIsIntersect(a, b, p2, p4) ||
                       GeoUtils.LineIsIntersect(a, b, p4, p3) ||
                       GeoUtils.LineIsIntersect(a, b, p3, p1);
            }
            
            if (Intersect(t1, t2) || Intersect(t2, t3) || Intersect(t3, t1)) return true;

            return false;
        }
        
        private bool PointInTriangle(Vector3F point, int tId) {
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
        
        private FloatF PointToTriangleDistance(Vector3F point, int tId) {
            Vector3F vId1 = data.vertices[data.indices[tId * 3]];
            Vector3F vId2 = data.vertices[data.indices[tId * 3 + 1]];
            Vector3F vId3 = data.vertices[data.indices[tId * 3 + 2]];
            FloatF d1 = GeoUtils.PointToSegment(point, vId1, vId2);
            FloatF d2 = GeoUtils.PointToSegment(point, vId2, vId3);
            FloatF d3 = GeoUtils.PointToSegment(point, vId3, vId1);
            return FloatF.Min(d1, FloatF.Min(d2, d3));
        }
        
        private (int, int) GetAABBIndex(Vector3F point) {
            int row = (int)((point - min).x / size.x);
            int col = (int)((point - min).z / size.z);
            return (row, col);
        }
    }
}