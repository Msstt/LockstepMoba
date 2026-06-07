// 默认地图正方形，且行列相同

using System;
using System.Collections.Generic;
using System.Linq;

namespace Combat.Fog {
    public abstract class Vision {
        private VisionBlockerMap blockerMap;

        private int[][] visionData;
        protected int visionCount = 0;
        
        public int VisionCount => visionCount;

        protected Vision(VisionBlockerMap blockerMap) {
            this.blockerMap = blockerMap;
            
            ArrayUtils.InitArray(ref visionData, FogConfig.VisionCellCount, FogConfig.VisionCellCount);
            ArrayUtils.InitArray(ref visitedCell, 2 * FogConfig.VisionCellCount, 2 * FogConfig.VisionCellCount);
        }

        public Action AddVision(Vector3F position, FloatF rowRadius) {
            int radius = (int)(rowRadius / blockerMap.CellSize.x);
            if (radius < 1) {
                return () => {};
            }
            var cellList = GetCell(position, radius);
            foreach (var (x, y) in cellList) {
                visionData[x][y] += 1;
            }
            visionCount += 1;
            return () => {
                foreach (var (x, y) in cellList) {
                    visionData[x][y] -= 1;
                }
                visionCount -= 1;
            };
        }

        public bool IsVisible(int x, int y) {
            if (visionData == null || x < 0 || x >= FogConfig.VisionCellCount || y < 0 || y >= FogConfig.VisionCellCount) {
                return false;
            }
            return IsVisible(visionData[x][y]);
        }

        protected abstract bool IsVisible(int data);

        private bool IsBlocked(int x, int y) {
            if (blockerMap != null && x >= 0 && blockerMap.Blocker.Length > x && y >= 0 && blockerMap.Blocker[x].Length > y) {
                return blockerMap.Blocker[x][y];
            }
            return false;
        }

        private bool[][] visitedCell;
        private List<(int, int)> GetCell(Vector3F position, int radius) {
            var (row, col) = GetCellIndex(position);
            var cellList = GetCircleCell(radius);
            List<(int, int)> result = new List<(int, int)>();
            foreach (var (x, y) in cellList) {
                if (visitedCell[x + FogConfig.VisionCellCount][y + FogConfig.VisionCellCount]) {
                    continue;
                }
                var lineCellList = GetLineCell(x, y);
                bool blocked = false;
                foreach (var (lineX, lineY) in lineCellList) {
                    if (IsBlocked(row + lineX, col + lineY)) {
                        blocked = true;
                    }
                    if (!blocked) {
                        result.Add((row + lineX, col + lineY));
                    }
                    visitedCell[lineX + FogConfig.VisionCellCount][lineY + FogConfig.VisionCellCount] = true;
                }
            }
            foreach (var (x, y) in cellList) {
                visitedCell[x + FogConfig.VisionCellCount][y + FogConfig.VisionCellCount] = false;
            }
            result = result.OrderBy(t => t.Item1).ThenBy(t => t.Item2).Distinct().ToList();
            result.RemoveAll(t =>
                t.Item1 < 0 || t.Item1 >= FogConfig.VisionCellCount || t.Item2 < 0 ||
                t.Item2 >= FogConfig.VisionCellCount);
            return result;
        }
        
        private readonly Dictionary<int, List<(int, int)>> circleCellCache = new Dictionary<int, List<(int, int)>>();
        private List<(int, int)> GetCircleCell(int radius) {
            if (circleCellCache.TryGetValue(radius, out List<(int, int)> cellList)) {
                return cellList;
            }

            cellList = new List<(int, int)>();
            for (int x = -radius; x <= radius; x++) {
                for (int y = -radius; y <= radius; y++) {
                    if (x * x + y * y <= radius * radius) {
                        cellList.Add((x, y));
                    }
                }
            }

            cellList.Sort((a, b) => -(a.Item1 * a.Item1 + a.Item2 * a.Item2 - b.Item1 * b.Item1 - b.Item2 * b.Item2));
            circleCellCache[radius] = cellList;
            return cellList;
        }

        private readonly Dictionary<int, List<(int, int)>> lineCellCache = new Dictionary<int, List<(int, int)>>();
        private List<(int, int)> GetLineCell(int x, int y) {
            if (lineCellCache.TryGetValue(x * 10000 + y, out List<(int, int)> cellList)) {
                return cellList;
            }
            
            cellList = new List<(int, int)>();
            int absX = Math.Abs(x);
            int absY = Math.Abs(y);
            int stepX = x >= 0 ? 1 : -1;
            int stepY = y >= 0 ? 1 : -1;
            if (absX == 0 && absY == 0) {
                cellList.Add((0, 0));
            } else if (absX > absY) {
                FloatF deltaError = new FloatF(2 * absY) / new FloatF(absX);
                FloatF error = 0;
                int j = 0;
                for (int i = 0; i != x; i += stepX) {
                    cellList.Add((i, j));
                    error += deltaError;
                    if (error >= FloatF.one) {
                        j += stepY;
                        error -= FloatF.two;
                    }
                }
            } else {
                FloatF deltaError = new FloatF(2 * absX) / new FloatF(absY);
                FloatF error = 0;
                int j = 0;
                for (int i = 0; i != y; i += stepY) {
                    cellList.Add((j, i));
                    error += deltaError;
                    if (error >= FloatF.one) {
                        j += stepX;
                        error -= FloatF.two;
                    }
                }
            }

            foreach (var cell in cellList) {
                if (cell.Item1 * cell.Item1 + cell.Item2 * cell.Item2 > x * x + y * y) {
                    throw new CombatException($"GetLineCell error: cell {cell} is out of line ({x}, {y})");
                }
            }
            if (cellList.Last().Item1 != x || cellList.Last().Item2 != y) {
                cellList.Add((x, y));
            }
            lineCellCache[x * 10000 + y] = cellList;
            return cellList;
        }
        
        private (int, int) GetCellIndex(Vector3F position) {
            position += blockerMap.CellSize / FloatF.two;
            int x = (int)((position.x - blockerMap.Start.x) / blockerMap.CellSize.x);
            int y = (int)((position.z - blockerMap.Start.z) / blockerMap.CellSize.z);
            x = Math.Min(Math.Max(x, 0), FogConfig.VisionCellCount - 1);
            y = Math.Min(Math.Max(y, 0), FogConfig.VisionCellCount - 1);
            return (x, y);
        }
    }

    public class OrVision : Vision {
        public OrVision(VisionBlockerMap blockerMap) : base(blockerMap) {}

        protected override bool IsVisible(int data) => data > 0;
    }
    
    public class AndVision : Vision {
        public AndVision(VisionBlockerMap blockerMap) : base(blockerMap) {}

        protected override bool IsVisible(int data) => data >= visionCount;
    }
}
