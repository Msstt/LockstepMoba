// AABB 优化单位检测，需要将单位手动注册到系统
// TODO: 支持体积

using System;
using System.Collections.Generic;
using Framework;

namespace Navmesh {
    public class UnitRaycaster {
        public static int MaxTypeCount = 20;
        private static int MaxAABBCount = 100;

        private class UnitInfo {
            public int id;
            public int type;
            public Vector3F pos;
            public int index;
            public Action<Vector3F> callback;
        }
        
        private Dictionary<int, UnitInfo> unitInfos;
        private HashSet<int>[][] aabbGrids;

        private Vector3F min, max;
        private Vector3F size;

        public void Init(Vector3F min, Vector3F max) {
            this.min = min;
            this.max = max;
            size = new Vector3F((max.x - min.x) / MaxAABBCount, (max.y - min.y) / MaxAABBCount, (max.z - min.z) / MaxAABBCount);
            
            unitInfos = new Dictionary<int, UnitInfo>();
            aabbGrids = new HashSet<int>[MaxTypeCount][];
            for (int type = 0; type < MaxTypeCount; type++) {
                aabbGrids[type] = new HashSet<int>[MaxAABBCount * MaxAABBCount];
                for (int i = 0; i < MaxAABBCount * MaxAABBCount; i++) {
                    aabbGrids[type][i] = new HashSet<int>();
                }
            }
        }

        private int GetIndex(Vector3F pos) {
            int row = (int)((pos.x - min.x) / size.x);
            int col = (int)((pos.z - min.z) / size.z);
            row = Math.Min(Math.Max(row, 0), MaxAABBCount - 1);
            col = Math.Min(Math.Max(col, 0), MaxAABBCount - 1);
            return row * MaxAABBCount + col;
        }

        private void ChangePos(int id, Vector3F pos) {
            if (!unitInfos.TryGetValue(id, out UnitInfo unitInfo)) {
                return;
            }
            unitInfo.pos = pos;
            int newIndex = GetIndex(pos);
            if (newIndex == unitInfo.index) {
                return;
            }
            aabbGrids[unitInfo.type][unitInfo.index].Remove(id);
            unitInfo.index = newIndex;
            aabbGrids[unitInfo.type][newIndex].Add(id);
        }
        
        public void Register(int id, int type, Vector3F pos, SafeEvent<Vector3F> onPosChange) {
            if (type >= MaxTypeCount) {
                Log.Error("Type exceeds max count: " + type);
                return;
            }
            if (unitInfos.ContainsKey(id)) {
                Log.Warning("Unit already registered: " + id);
                return;
            }
            int index = GetIndex(pos);
            aabbGrids[type][index].Add(id);
            unitInfos[id] = new UnitInfo() {
                id = id,
                type = type,
                pos = pos,
                index = index,
                callback = (newPos) => {
                    ChangePos(id, newPos);
                }
            };
            onPosChange.Register(unitInfos[id].callback);
        }
        
        public void UnRegister(int id, SafeEvent<Vector3F> onPosChange) {
            if (!unitInfos.TryGetValue(id, out UnitInfo unitInfo)) {
                return;
            }
            onPosChange.UnRegister(unitInfo.callback);
            aabbGrids[unitInfo.type][unitInfo.index].Remove(id);
            unitInfos.Remove(id);
        }

        private (int, int) GetRowCol(Vector3F pos) {
            int row = (int)((pos.x - min.x) / size.x);
            int col = (int)((pos.z - min.z) / size.z);
            row = Math.Min(Math.Max(row, 0), MaxAABBCount - 1);
            col = Math.Min(Math.Max(col, 0), MaxAABBCount - 1);
            return (row, col);
        }

        private void IterateGrid(int type, Vector3F min, Vector3F max, Action<UnitInfo> func) {
            var (row1, col1) = GetRowCol(min);
            var (row2, col2) = GetRowCol(max);
            for (int row = row1; row <= row2; row++) {
                for (int col = col1; col <= col2; col++) {
                    int index = row * MaxAABBCount + col;
                    foreach (var id in aabbGrids[type][index]) {
                        if (unitInfos.TryGetValue(id, out var unitInfo)) {
                            func(unitInfo);
                        }
                    }
                }
            }
        }

        private void IterateType(int typeBitSet, Action<int> func) {
            for (int i = 0; i < MaxTypeCount; i++) {
                if ((typeBitSet & (1 << i)) == 0) {
                    continue;
                }
                func(i);
            }
        }
        
        public void RaycastInCircle(int typeBitSet, Vector3F center, FloatF radius, List<int> result) {
            result.Clear();
            void Check(UnitInfo unitInfo) {
                if (Vector3F.Distance2(unitInfo.pos, center) <= radius * radius) {
                    result.Add(unitInfo.id);
                }
            }
            Vector3F min = center - new Vector3F(radius, 0, radius);
            Vector3F max = center + new Vector3F(radius, 0, radius);
            IterateType(typeBitSet, (type) => {
                IterateGrid(type, min, max, Check);
            });
        }
        
        public void RaycastInPolygon(int typeBitSet, List<Vector3F> polygon, List<int> result) {
            result.Clear();
            if (polygon.Count < 3) {
                return;
            }
            void Check(UnitInfo unitInfo) {
                if (GeoUtils.PointInPolygon(unitInfo.pos, polygon)) {
                    result.Add(unitInfo.id);
                }
            }
            Vector3F min = new Vector3F(FloatF.max, FloatF.max, FloatF.max);
            Vector3F max = new Vector3F(FloatF.min, FloatF.min, FloatF.min);
            foreach (var point in polygon) {
                min.x = FloatF.Min(min.x, point.x);
                min.y = FloatF.Min(min.y, point.y);
                min.z = FloatF.Min(min.z, point.z);
                max.x = FloatF.Max(max.x, point.x);
                max.y = FloatF.Max(max.y, point.y);
                max.z = FloatF.Max(max.z, point.z);
            }
            IterateType(typeBitSet, (type) => {
                IterateGrid(type, min, max, Check);
            });
        }
        
        public void RaycastInRect(int typeBitSet, Vector3F center, Vector3F direction, FloatF length, FloatF width, List<int> result) {
            result.Clear();
            Vector3F forward = direction.Normalized();
            if (forward == Vector3F.zero) {
                forward = new Vector3F(0, 0, 1);
            }
            Vector3F right = new Vector3F(forward.z, 0, -forward.x);
            Vector3F halfForward = forward * (length / 2);
            Vector3F halfRight = right * (width / 2);
            FloatF halfLength = length / 2;
            FloatF halfWidth = width / 2;
            
            void Check(UnitInfo unitInfo) {
                Vector3F offset = unitInfo.pos - center;
                if (FloatF.Abs(Vector3F.Dot(offset, forward)) <= halfLength &&
                    FloatF.Abs(Vector3F.Dot(offset, right)) <= halfWidth) {
                    result.Add(unitInfo.id);
                }
            }
            
            Vector3F point1 = center + halfForward + halfRight;
            Vector3F point2 = center + halfForward - halfRight;
            Vector3F point3 = center - halfForward - halfRight;
            Vector3F point4 = center - halfForward + halfRight;
            Vector3F min = new Vector3F(FloatF.max, FloatF.max, FloatF.max);
            Vector3F max = new Vector3F(FloatF.min, FloatF.min, FloatF.min);
            min.x = FloatF.Min(FloatF.Min(point1.x, point2.x), FloatF.Min(point3.x, point4.x));
            min.y = FloatF.Min(FloatF.Min(point1.y, point2.y), FloatF.Min(point3.y, point4.y));
            min.z = FloatF.Min(FloatF.Min(point1.z, point2.z), FloatF.Min(point3.z, point4.z));
            max.x = FloatF.Max(FloatF.Max(point1.x, point2.x), FloatF.Max(point3.x, point4.x));
            max.y = FloatF.Max(FloatF.Max(point1.y, point2.y), FloatF.Max(point3.y, point4.y));
            max.z = FloatF.Max(FloatF.Max(point1.z, point2.z), FloatF.Max(point3.z, point4.z));
            IterateType(typeBitSet, (type) => {
                IterateGrid(type, min, max, Check);
            });
        }
    }
}
