using System.Collections.Generic;
using System.IO;
using System.Linq;
using Combat.Area;
using Editor.Area;
using Framework;
using UnityEditor;
using UnityEngine;

namespace Editor.Node {
    public partial class NodeEditor {
        private class Area : INodeEditor {
            private class AreaData {
                public int Id;
                public string Name;

                public AreaData(AreaConfig config) {
                    Id = config.Id;
                    Name = config.Name;
                }

                public AreaData(int id) {
                    Id = id;
                }
            }
            
            private List<AreaData> data;

            public Area() {
                Import();
            }

            private void Import() {
                data = new List<AreaData>();
                var areaFiles = Directory.GetFiles(AreaGraph.ExportPath, "*.json", SearchOption.AllDirectories);
                foreach (var file in areaFiles) {
                    if (JsonHelper.LoadFromFile<AreaConfig>(file, out var config)) {
                        data.Add(new AreaData(config));
                    }
                }
                data.Sort((a, b) => a.Id.CompareTo(b.Id));
            }
            
            public int DataCount => data.Count;

            public void DrawHeader() {
                GUILayout.Label("ID", GUILayout.Width(50));
                GUILayout.Label("名称", GUILayout.Width(150));
                GUILayout.Label("类型", GUILayout.Width(100));
            }

            public void DrawData(int index) {
                GUIStyle cellStyle = new GUIStyle(EditorStyles.label) {
                    alignment = TextAnchor.MiddleLeft
                };
                AreaData area = data[index];
                GUILayout.Label(area.Id.ToString(), cellStyle, GUILayout.Width(50));
                GUILayout.Label(area.Name, cellStyle, GUILayout.Width(150));
            }
            
            private int GenerateId() {
                return data.Count > 0 ? data[data.Count - 1].Id + 1 : 1;
            }

            public void Create() {
                int id = GenerateId();
                AreaGraph graph = ScriptableObject.CreateInstance<AreaGraph>();
                RootNode root = graph.AddNode<RootNode>();
                root.config.Id = id;
                AssetDatabase.CreateAsset(graph, AreaGraph.ImportRelaPath + id + ".asset");
                AssetDatabase.SaveAssets();
            
                graph.Export(true);
            
                data.Add(new AreaData(id));
            }

            public void Edit(int index) {
                int id = data[index].Id;
                var graph = AssetDatabase.LoadAssetAtPath<AreaGraph>(AreaGraph.ImportRelaPath + id + ".asset");
                graph.OnExportEnd = () => {
                    if (!JsonHelper.LoadFromFile<AreaConfig>(AreaGraph.ExportPath + id + ".json", out var config)) {
                        return;
                    }
                    data[index] = new AreaData(config);
                };
                NodeCanvas.Editor.GraphEditor.OpenWindow(graph);
            }

            public void Copy(int index) {
                int targetId = data[index].Id;
                int id = GenerateId();
                string targetPath = AreaGraph.ImportRelaPath + targetId + ".asset";
                string copyPath = AreaGraph.ImportRelaPath + id + ".asset";
                if (!AssetDatabase.CopyAsset(targetPath, copyPath)) {
                    AreaGraph.ExportError("复制资源失败");
                    return;
                }
                var graph = AssetDatabase.LoadAssetAtPath<AreaGraph>(copyPath);
                if (graph == null || !graph.GetAllNodesOfType<RootNode>().Any()) {
                    AreaGraph.ExportError("缺少Root节点");
                    return;
                }
                var node = graph.GetAllNodesOfType<RootNode>().First();
                node.config.Id = id;
                graph.name = id.ToString();
                EditorUtility.SetDirty(graph);
                AssetDatabase.SaveAssets();

                graph.Export();
            
                if (!JsonHelper.LoadFromFile<AreaConfig>(AreaGraph.ExportPath + id + ".json", out var config)) {
                    return;
                }
                data.Add(new AreaData(config));
            }

            public void Delete(List<int> index) {
                if (!index.Any()) {
                    return;
                }
                foreach (var i in index) {
                    File.Delete(AreaGraph.ImportPath + data[i].Id + ".asset");
                    File.Delete(AreaGraph.ExportPath + data[i].Id + ".json");
                }
                foreach (var i in index) {
                    data.RemoveAt(i);
                }
            }

            public void Refresh() {
                Import();
            }

            public void ExportAll() {
                foreach (var areaData in data) {
                    var graph = AssetDatabase.LoadAssetAtPath<AreaGraph>(AreaGraph.ImportRelaPath + areaData.Id + ".asset");
                    graph.Export();
                }
            }
        }
    }
}
