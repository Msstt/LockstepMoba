using System.Collections.Generic;
using System.IO;
using System.Linq;
using Combat.Buff;
using Editor.Buff;
using Framework;
using UnityEditor;
using UnityEngine;

namespace Editor.Node {
    public partial class NodeEditor {
        private class Buff : INodeEditor {
            private class BuffData {
                public int Id;
                public string Name;

                public BuffData(BuffConfig config) {
                    Id = config.Id;
                    Name = config.Name;
                }

                public BuffData(int id) {
                    Id = id;
                }
            }
            
            private List<BuffData> data;

            public Buff() {
                Import();
            }

            private void Import() {
                data = new List<BuffData>();
                var buffFiles = Directory.GetFiles(BuffGraph.ExportPath, "*.json", SearchOption.AllDirectories);
                foreach (var file in buffFiles) {
                    if (JsonHelper.LoadFromFile<BuffConfig>(file, out var config)) {
                        data.Add(new BuffData(config));
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
                BuffData buff = data[index];
                GUILayout.Label(buff.Id.ToString(), cellStyle, GUILayout.Width(50));
                GUILayout.Label(buff.Name, cellStyle, GUILayout.Width(150));
            }
            
            private int GenerateId() {
                return data.Count > 0 ? data[data.Count - 1].Id + 1 : 1;
            }

            public void Create() {
                int id = GenerateId();
                BuffGraph graph = ScriptableObject.CreateInstance<BuffGraph>();
                RootNode root = graph.AddNode<RootNode>();
                root.config.Id = id;
                AssetDatabase.CreateAsset(graph, BuffGraph.ImportRelaPath + id + ".asset");
                AssetDatabase.SaveAssets();
            
                graph.Export(true);
            
                data.Add(new BuffData(id));
            }

            public void Edit(int index) {
                int id = data[index].Id;
                var graph = AssetDatabase.LoadAssetAtPath<BuffGraph>(BuffGraph.ImportRelaPath + id + ".asset");
                graph.OnExportEnd = () => {
                    if (!JsonHelper.LoadFromFile<BuffConfig>(BuffGraph.ExportPath + id + ".json", out var config)) {
                        return;
                    }
                    data[index] = new BuffData(config);
                };
                NodeCanvas.Editor.GraphEditor.OpenWindow(graph);
            }

            public void Copy(int index) {
                int targetId = data[index].Id;
                int id = GenerateId();
                File.Copy(BuffGraph.ImportPath + targetId + ".asset", BuffGraph.ImportPath + id + ".asset");
                AssetDatabase.Refresh();
                var graph = AssetDatabase.LoadAssetAtPath<BuffGraph>(BuffGraph.ImportRelaPath + id + ".asset");
                if (!graph.GetAllNodesOfType<RootNode>().Any()) {
                    BuffGraph.ExportError("缺少Root节点");
                    return;
                }
                var node = graph.GetAllNodesOfType<RootNode>().First();
                node.config.Id = id;
            
                graph.Export();
            
                if (!JsonHelper.LoadFromFile<BuffConfig>(BuffGraph.ExportPath + id + ".json", out var config)) {
                    return;
                }
                data.Add(new BuffData(config));
            }

            public void Delete(List<int> index) {
                if (!index.Any()) {
                    return;
                }
                foreach (var i in index) {
                    File.Delete(BuffGraph.ImportPath + data[i].Id + ".asset");
                    File.Delete(BuffGraph.ExportPath + data[i].Id + ".json");
                }
                foreach (var i in index) {
                    data.RemoveAt(i);
                }
            }

            public void Refresh() {
                Import();
            }

            public void ExportAll() {
                foreach (var buffData in data) {
                    var graph = AssetDatabase.LoadAssetAtPath<BuffGraph>(BuffGraph.ImportRelaPath + buffData.Id + ".asset");
                    graph.Export();
                }
            }
        }
    }
}