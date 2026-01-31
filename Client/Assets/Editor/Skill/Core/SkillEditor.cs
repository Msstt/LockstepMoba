using System.Collections.Generic;
using System.IO;
using System.Linq;
using Combat.Skill;
using Framework;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.Skill {
    public class SkillEditor : OdinEditorWindow {
        private List<SkillData> data = new();

        private Vector2 dataViewScroll = Vector2.zero;

        [MenuItem("工具/技能编辑器")]
        static void Open() {
            SkillEditor window = GetWindow<SkillEditor>("技能编辑器", true);
            window.minSize = new Vector2(800f, 600f);
            window.data = SkillDefineUtils.Import();
        }

        private void OnGUI() {
            DrawHeader();
            DrawDataList();
        }

        GUIStyle rowStyle;
        GUIStyle cellStyle;

        private void InitStyles() {
            if (rowStyle != null) return;

            rowStyle = new GUIStyle(EditorStyles.helpBox) {
                padding = new RectOffset(6, 6, 4, 4),
                margin = new RectOffset(2, 2, 2, 2)
            };

            cellStyle = new GUIStyle(EditorStyles.label) {
                alignment = TextAnchor.MiddleLeft
            };
        }

        private void DrawHeader() {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
                if (GUILayout.Button("新建技能", GUILayout.Width(80))) {
                    CreateSkill();
                }
                if (GUILayout.Button("刷新", GUILayout.Width(50))) {
                    Refresh();
                }
                if (GUILayout.Button("全量导出", GUILayout.Width(80))) {
                    ExportAll();
                }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
                GUILayout.Label("ID", GUILayout.Width(50));
                GUILayout.Label("名称", GUILayout.Width(200));
                GUILayout.Label("类型", GUILayout.Width(100));
                GUILayout.FlexibleSpace();
                GUILayout.Label("操作", GUILayout.Width(120));
            GUILayout.EndHorizontal();
        }

        private void DrawDataList() {
            InitStyles();

            dataViewScroll = GUILayout.BeginScrollView(dataViewScroll);

            List<int> toRemove = new List<int>();

            for (int i = 0; i < data.Count; i++) {
                var skill = data[i];

                GUILayout.BeginHorizontal(rowStyle);

                GUILayout.Label(skill.Id.ToString(), cellStyle, GUILayout.Width(50));
                GUILayout.Label(skill.Name, cellStyle, GUILayout.Width(200));
                GUILayout.Label(skill.Type.ToString(), cellStyle, GUILayout.Width(100));

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("编辑", GUILayout.Width(50))) {
                    EditSkill(i);
                }
                
                if (GUILayout.Button("复制", GUILayout.Width(50))) {
                    CopySkill(skill.Id);
                }

                if (GUILayout.Button("删除", GUILayout.Width(50))) {
                    bool ok = EditorUtility.DisplayDialog(
                        "确认操作",
                        "你确定要删除此技能吗？",
                        "确定",
                        "取消"
                    );
                    if (ok) {
                        toRemove.Add(i);
                    }
                }

                GUILayout.EndHorizontal();
            }
            DeleteSkill(toRemove);
            
            GUILayout.EndScrollView();
        }

        private int GenerateId() {
            return data.Count > 0 ? data[data.Count - 1].Id + 1 : 1;
        }

        private void CreateSkill() {
            int id = GenerateId();
            SkillGraph graph = ScriptableObject.CreateInstance<SkillGraph>();
            RootNode root = graph.AddNode<RootNode>();
            root.config.Id = id;
            AssetDatabase.CreateAsset(graph, "Assets/Resources/Config/Skill/Node/" + id + ".asset");
            AssetDatabase.SaveAssets();
            
            graph.Export(true);
            
            data.Add(new SkillData(id));
            SkillDefineUtils.Export(data);
        }

        private void DeleteSkill(List<int> toRemove) {
            if (!toRemove.Any()) {
                return;
            }
            foreach (var i in toRemove) {
                File.Delete(SkillGraph.ImportPath + data[i].Id + ".asset");
                File.Delete(SkillGraph.ExportPath + data[i].Id + ".json");
            }
            foreach (var i in toRemove) {
                data.RemoveAt(i);
            }
            SkillDefineUtils.Export(data);
        }

        private void EditSkill(int index) {
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

        private void CopySkill(int targetId) {
            int id = GenerateId();
            File.Copy(SkillGraph.ImportPath + targetId + ".asset", SkillGraph.ImportPath + id + ".asset");
            AssetDatabase.Refresh();
            var graph = AssetDatabase.LoadAssetAtPath<SkillGraph>(SkillGraph.ImportRelaPath + id + ".asset");
            if (!graph.GetAllNodesOfType<RootNode>().Any()) {
                SkillGraph.ExportError("缺少Root节点");
                return;
            }
            var node = graph.GetAllNodesOfType<RootNode>().First();
            node.config.Id = id;
            
            graph.Export();
            
            if (!JsonHelper.LoadFromFile<SkillConfig>(SkillGraph.ExportPath + id + ".json", out var config)) {
                return;
            }
            data.Add(new SkillData(config));
            SkillDefineUtils.Export(data);
        }

        private void Refresh() {
            data = SkillDefineUtils.Refresh();
        }
        
        private void ExportAll() {
            foreach (var skillData in data) {
                var graph = AssetDatabase.LoadAssetAtPath<SkillGraph>(SkillGraph.ImportRelaPath + skillData.Id + ".asset");
                graph.Export();
            }

            Refresh();
        }
    }
}