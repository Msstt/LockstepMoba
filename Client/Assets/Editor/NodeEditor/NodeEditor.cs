using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.Node {
    public interface INodeEditor {
        public int DataCount { get; }
        public void DrawHeader();
        public void DrawData(int index);

        public void Create();
        public void Edit(int index);
        public void Copy(int index);
        public void Delete(List<int> index);
        public void Refresh();
        public void ExportAll();
    }
    
    public partial class NodeEditor : OdinEditorWindow {
        private static readonly string[] Type = new[] {
            "技能",
            "Buff",
            "区域",
        };

        private Vector2 dataViewScroll = Vector2.zero;
        
        private INodeEditor[] editors = new INodeEditor[] {
            new Skill(),
            new Buff(),
            new Area(),
        };
        private int skillEditorType = 1;

        [MenuItem("工具/技能编辑器")]
        public static void Open() {
            NodeEditor window = GetWindow<NodeEditor>("技能编辑器", true);
            window.minSize = new Vector2(800f, 600f);
        }

        private INodeEditor CurEditor => editors[skillEditorType];

        private void OnGUI() {
            DrawHeader();
            DrawDataList();
        }

        private void DrawHeader() {
            skillEditorType = GUILayout.Toolbar(skillEditorType, Type, EditorStyles.toolbarButton);
            
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
                if (GUILayout.Button("新建", GUILayout.Width(50))) {
                    CurEditor.Create();
                }
                if (GUILayout.Button("刷新", GUILayout.Width(50))) {
                    CurEditor.Refresh();
                }
                if (GUILayout.Button("全量导出", GUILayout.Width(80))) {
                    CurEditor.ExportAll();
                    CurEditor.Refresh();
                }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
                CurEditor.DrawHeader();
                GUILayout.FlexibleSpace();
                GUILayout.Label("操作", GUILayout.Width(120));
            GUILayout.EndHorizontal();
        }

        private void DrawDataList() {
            dataViewScroll = GUILayout.BeginScrollView(dataViewScroll);

            int count = CurEditor.DataCount;
            List<int> toRemove = new List<int>();

            for (int i = 0; i < count; i++) {
                GUILayout.BeginHorizontal(new GUIStyle(EditorStyles.helpBox) {
                    padding = new RectOffset(6, 6, 4, 4),
                    margin = new RectOffset(2, 2, 2, 2)
                });
                    CurEditor.DrawData(i);
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("编辑", GUILayout.Width(50))) {
                    CurEditor.Edit(i);
                }
                
                if (GUILayout.Button("复制", GUILayout.Width(50))) {
                    CurEditor.Copy(i);
                }

                if (GUILayout.Button("删除", GUILayout.Width(50))) {
                    bool ok = EditorUtility.DisplayDialog(
                        "确认操作",
                        "你确定要删除此配置吗？",
                        "确定",
                        "取消"
                    );
                    if (ok) {
                        toRemove.Add(i);
                    }
                }

                GUILayout.EndHorizontal();
            }
            CurEditor.Delete(toRemove);
            
            GUILayout.EndScrollView();
        }
    }
}