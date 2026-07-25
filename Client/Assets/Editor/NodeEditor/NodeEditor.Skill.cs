using System.Collections.Generic;
using System.IO;
using System.Linq;
using Combat.Skill;
using Editor.Skill;
using Framework;
using UnityEditor;
using UnityEngine;

namespace Editor.Node {
    public partial class NodeEditor {
        private class Skill : INodeEditor {
            private List<SkillData> data;

            public Skill() {
                data = SkillDefineUtils.Import();
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
                SkillData skill = data[index];
                GUILayout.Label(skill.Id.ToString(), cellStyle, GUILayout.Width(50));
                GUILayout.Label(skill.Name, cellStyle, GUILayout.Width(150));
                GUILayout.Label(skill.Type.ToString(), cellStyle, GUILayout.Width(100));
            }
            
            private int GenerateId() {
                return data.Count > 0 ? data[data.Count - 1].Id + 1 : 1;
            }

            public void Create() {
                int id = GenerateId();
                SkillGraph graph = ScriptableObject.CreateInstance<SkillGraph>();
                RootNode root = graph.AddNode<RootNode>();
                root.config.Id = id;
                AssetDatabase.CreateAsset(graph, SkillGraph.ImportRelaPath + id + ".asset");
                AssetDatabase.SaveAssets();
            
                graph.Export(true);
            
                data.Add(new SkillData(id));
                SkillDefineUtils.Export(data);
            }

            public void Edit(int index) {
                int id = data[index].Id;
                var graph = AssetDatabase.LoadAssetAtPath<SkillGraph>(SkillGraph.ImportRelaPath + id + ".asset");
                graph.OnExportEnd = () => {
                    if (!JsonHelper.LoadFromFile<SkillConfig>(SkillGraph.ExportPath + id + ".json", out var config)) {
                        return;
                    }
                    data[index] = new SkillData(config);
                    SkillDefineUtils.Export(data);
                };
                NodeCanvas.Editor.GraphEditor.OpenWindow(graph);
            }

            public void Copy(int index) {
                int targetId = data[index].Id;
                int id = GenerateId();
                string targetPath = SkillGraph.ImportRelaPath + targetId + ".asset";
                string copyPath = SkillGraph.ImportRelaPath + id + ".asset";
                if (!AssetDatabase.CopyAsset(targetPath, copyPath)) {
                    SkillGraph.ExportError("复制资源失败");
                    return;
                }
                var graph = AssetDatabase.LoadAssetAtPath<SkillGraph>(copyPath);
                if (graph == null || !graph.GetAllNodesOfType<RootNode>().Any()) {
                    SkillGraph.ExportError("缺少Root节点");
                    return;
                }
                var node = graph.GetAllNodesOfType<RootNode>().First();
                node.config.Id = id;
                graph.name = id.ToString();
                EditorUtility.SetDirty(graph);
                AssetDatabase.SaveAssets();

                graph.Export();
            
                if (!JsonHelper.LoadFromFile<SkillConfig>(SkillGraph.ExportPath + id + ".json", out var config)) {
                    return;
                }
                data.Add(new SkillData(config));
                SkillDefineUtils.Export(data);
            }

            public void Delete(List<int> index) {
                if (!index.Any()) {
                    return;
                }
                foreach (var i in index) {
                    File.Delete(SkillGraph.ImportPath + data[i].Id + ".asset");
                    File.Delete(SkillGraph.ExportPath + data[i].Id + ".json");
                }
                foreach (var i in index) {
                    data.RemoveAt(i);
                }
                SkillDefineUtils.Export(data);
            }

            public void Refresh() {
                data = SkillDefineUtils.Refresh();
            }

            public void ExportAll() {
                foreach (var skillData in data) {
                    var graph = AssetDatabase.LoadAssetAtPath<SkillGraph>(SkillGraph.ImportRelaPath + skillData.Id + ".asset");
                    graph.Export();
                }
            }
        }
    }
}
