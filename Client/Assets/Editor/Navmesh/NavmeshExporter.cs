using System.Collections.Generic;
using System.IO;
using Framework;
using Navmesh;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Editor.Network {
    public class NavmeshExporter {
        private static string assetPath = Application.dataPath + "/Scenes/NavmeshData/";
        private static string assetRePath = "Assets/Scenes/NavmeshData/";
        
        [MenuItem("工具/Navmesh/导出 Navmesh 网格数据")]
        public static void Execute() {
            var protoFiles = Directory.GetFiles(assetPath, "*.asset", SearchOption.AllDirectories);
            Dictionary<FloatF, NavmeshSurface> surfaces = new Dictionary<FloatF, NavmeshSurface>();
            foreach (var file in protoFiles) {
                var data = AssetDatabase.LoadAssetAtPath<NavMeshData>(assetRePath + Path.GetFileName(file));
                if (data == null) {
                    continue;
                }
                var instance = NavMesh.AddNavMeshData(data);
                var settings = NavMesh.GetSettingsByID(int.Parse(Path.GetFileNameWithoutExtension(file)));
                surfaces[settings.agentRadius] = ExportNavmeshData();
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
            foreach (var vertice in tri.vertices) {
                surface.vertices.Add(new Vector3F(vertice.x, vertice.y, vertice.z));
            }
            foreach (var indice in tri.indices) {
                surface.indices.Add(indice);
            }
            return surface;
        }
    }
}