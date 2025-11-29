using System.Collections.Generic;
using System.IO;
using Framework;
using Navmesh;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Editor.Network {
    public class NavmeshExporter {
        [MenuItem("工具/Navmesh/导出 Navmesh 网格数据")]
        public static void Execute() {
            var selected = Selection.activeObject;
            if (selected == null) {
                Debug.LogError("请先选中文件夹");
                return;
            }
            string assetRePath = AssetDatabase.GetAssetPath(selected) + "/";
            string assetPath = Application.dataPath + assetRePath.Substring("Assets".Length);
            if (!Directory.Exists(assetPath)) {
                Debug.LogError("请先选中文件夹");
                return;
            }
            var protoFiles = Directory.GetFiles(assetPath, "*.asset", SearchOption.AllDirectories);
            Dictionary<FloatF, NavmeshSurface> surfaces = new Dictionary<FloatF, NavmeshSurface>();
            foreach (var file in protoFiles) {
                var data = AssetDatabase.LoadAssetAtPath<NavMeshData>(assetRePath + Path.GetFileName(file));
                if (data == null) {
                    continue;
                }
                var instance = NavMesh.AddNavMeshData(data);
                var settings = NavMesh.GetSettingsByID(int.Parse(Path.GetFileNameWithoutExtension(file)));
                surfaces[FloatF.FromFloat(settings.agentRadius)] = ExportNavmeshData();
                NavMesh.RemoveNavMeshData(instance);
            }
            if (!JsonHelper.SaveToFile(surfaces, assetPath + "navmesh_surfaces.json")) {
                return;
            }
            
            AssetDatabase.Refresh();
            Debug.Log("已生成所有网格数据");
        }
        
        private static NavmeshSurface ExportNavmeshData() {
            NavmeshSurface surface = new NavmeshSurface() {
                vertices = new List<Vector3F>(),
                indices = new List<int>(),
            };
            var tri = NavMesh.CalculateTriangulation();
            List<int> mapping = new List<int>();
            foreach (var vertice in tri.vertices) {
                var point = new Vector3F(FloatF.FromFloat(vertice.x), FloatF.FromFloat(vertice.y),
                    FloatF.FromFloat(vertice.z));
                bool has = false;
                for (int i = 0; i < surface.vertices.Count; i++) {
                    if (Vector3F.IsEqualInEps(surface.vertices[i], point)) {
                        has = true;
                        mapping.Add(i);
                        break;
                    }
                }
                if (!has) {
                    surface.vertices.Add(point);
                    mapping.Add(surface.vertices.Count - 1);
                }
            }
            foreach (var indice in tri.indices) {
                surface.indices.Add(mapping[indice]);
            }
            return surface;
        }
    }
}