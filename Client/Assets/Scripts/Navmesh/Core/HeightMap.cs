
using UnityEngine;

namespace Navmesh {
    public class HeightMap {
        public float[][] Value;
        public Vector3 Start;
        public Vector3 CellSize;
        
        public float Get(float x, float y) {
            var (row, col) = GetAABBIndex(x, y);
            float h1 = GetHeight(row, col);
            float h2 = GetHeight(row + 1, col);
            float h3 = GetHeight(row, col + 1);
            float h4 = GetHeight(row + 1, col + 1);

            float h5 = h1 + (h2 - h1) * (x - Start.x - row * CellSize.x) / CellSize.x;
            float h6 = h3 + (h4 - h3) * (x - Start.x - row * CellSize.x) / CellSize.x;
            float h = h5 + (h6 - h5) * (y - Start.z - col * CellSize.z) / CellSize.z;
            return h;
        }
        
        private (int, int) GetAABBIndex(float x, float y) {
            int row = (int)((x - Start.x) / CellSize.x);
            int col = (int)((y - Start.z) / CellSize.z);
            return (row, col);
        }
        
        private float GetHeight(int row, int col) {
            if (row < 0 || row >= Value.Length || col < 0 || col >= Value[0].Length) {
                return 0;
            }
            return Value[row][col];
        }
    }
}