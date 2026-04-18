using System.Collections.Generic;
using System.IO;
using System.Linq;
using Framework;
using Navmesh;
using UnityEditor;
using UnityEngine;

namespace Editor.Navmesh {
    public class GenerateHeightMap {
        [MenuItem("工具/Navmesh/生成高度图")]
        public static void Execute() {
            Dictionary<FloatF, NavmeshSurface> surface = null;
            string path = "";
            foreach (var obj in Selection.objects) {
                path = AssetDatabase.GetAssetPath(obj);
                if (!JsonHelper.LoadFromFile(path, out surface)) {
                    Log.Error("Navmesh SurfaceData parse failed");
                    break;
                }
            }
            if (surface == null) {
                Log.Error("未选择Navmesh SurfaceData");
                return;
            }
            var (min, max) = surface.First().Value.GetBorder();
            FloatF deltaX = (max.x - min.x) / FindPathConfig.HeightMapCellCount;
            FloatF deltaY = (max.z - min.z) / FindPathConfig.HeightMapCellCount;
            HeightMap map = new HeightMap();
            map.Start = min.ToVector3();
            map.CellSize = ((max - min) / FindPathConfig.HeightMapCellCount).ToVector3();
            map.Value = new float[FindPathConfig.HeightMapCellCount + 1][];
            int missCount = 0;
            for (int i = 0; i <= FindPathConfig.HeightMapCellCount; i++) {
                map.Value[i] = new float[FindPathConfig.HeightMapCellCount + 1];
                for (int j = 0; j <= FindPathConfig.HeightMapCellCount; j++) {
                    FloatF x = min.x + deltaX * i;
                    FloatF y = min.z + deltaY * j;
                    Ray ray = new Ray(new Vector3(x.ToFloat(), 1000, y.ToFloat()), new Vector3(0, -2000, 0));
                    if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, LayerMask.GetMask("Map"))) {
                        map.Value[i][j] = hitInfo.point.y;
                    } else {
                        map.Value[i][j] = 0;
                        missCount += 1;
                    }
                }
            }
            Log.Warning($"射线未击中地面数量: {missCount}");

            if (!JsonHelper.SaveToFile(map, Path.GetDirectoryName(path) + "/height_map.json")) {
                return;
            }
            Log.Info("生成高度图完成");
        }
    }
}