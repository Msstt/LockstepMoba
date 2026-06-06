using System.Collections.Generic;
using System.IO;
using System.Linq;
using Combat.Fog;
using Framework;
using Navmesh;
using UnityEditor;
using UnityEngine;

namespace Editor.Navmesh {
    public static class GenerateVisionBlockerMap {
        [MenuItem("工具/战争迷雾/生成视野遮罩图")]
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
            float deltaX = ((max.x - min.x) / FogConfig.VisionCellCount).ToFloat();
            float deltaY = ((max.z - min.z) / FogConfig.VisionCellCount).ToFloat();
            VisionBlockerMap map = new VisionBlockerMap();
            map.Blocker = new bool[FogConfig.VisionCellCount][];
            map.Start = min;
            map.CellSize = (max - min) / FogConfig.VisionCellCount;
            int blockerCount = 0;
            for (int i = 0; i < FogConfig.VisionCellCount; i++) {
                map.Blocker[i] = new bool[FogConfig.VisionCellCount];
                for (int j = 0; j < FogConfig.VisionCellCount; j++) {
                    float x = min.x.ToFloat() + deltaX * (i + 0.5f);
                    float y = min.z.ToFloat() + deltaY * (j + 0.5f);
                    Ray ray = new Ray(new Vector3(x, 1000, y), new Vector3(0, -2000, 0));
                    map.Blocker[i][j] = Physics.Raycast(ray, out RaycastHit _, Mathf.Infinity, LayerMask.GetMask("VisionBlocker"));
                    if (map.Blocker[i][j]) {
                        blockerCount += 1;
                    }
                }
            }
            Log.Info("生成视野遮罩图完成，遮罩比例：" + (float)blockerCount / (FogConfig.VisionCellCount * FogConfig.VisionCellCount));

            if (!JsonHelper.SaveToFile(map, Path.GetDirectoryName(path) + "/vision_blocker_map.json")) {
                return;
            }
            AssetDatabase.Refresh();
            Log.Info("生成视野遮罩图完成");
        }
    }
}