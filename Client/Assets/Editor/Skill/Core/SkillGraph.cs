using System;
using System.Collections.Generic;
using System.Linq;
using Combat;
using Combat.Skill;
using Framework;
using NodeCanvas.Framework;
using UnityEditor;
using UnityEngine;
using Node = NodeCanvas.Framework.Node;

namespace Editor.Skill {
    [CreateAssetMenu(menuName = "Skill/SkillGraph")]
    public class SkillGraph : Graph {
        private static readonly string ExportPath = Application.dataPath + "/Resources/Config/Skill/Json/";
        
        public override Type baseNodeType => typeof(SkillNode);
        public override bool requiresAgent => false;
        public override bool requiresPrimeNode => false;
        public override bool isTree => true;
        public override bool allowBlackboardOverrides => false;
        public override bool canAcceptVariableDrops => true;

        protected override void OnGraphEditorToolbar() {
            if (GUILayout.Button("Export")) {
                Export();
            }
        }
        
        public void Export() {
            RootNode root = null;
            foreach (var node in allNodes) {
                if (node is RootNode rootNode) {
                    if (root == null) {
                        root = rootNode;
                    } else {
                        ExportError("存在多个Root节点");
                        return;
                    }
                }
            }

            if (root == null) {
                ExportError("缺少Root节点");
                return;
            }
            
            SkillConfig config = new SkillConfig();
            config.Id = root.Id;
            config.SkillType = (int)root.SkillType;
            config.CanAbortSelf = root.CanAbortSelf;
            if (!root.GetChildNodes().Any()) {
                ExportError("Root节点缺少子节点");
                return;
            }
            config.Node = ExportNode(root.GetChildNodes().First() as SkillNode);

            JsonHelper.SaveToFile(config, ExportPath + config.Id + ".json");
            
            Debug.Log("技能导出成功: " + config.Id);
        }

        private NodeConfig ExportNode(SkillNode node) {
            if (node == null) {
                return null;
            }
            
            NodeConfig config = new NodeConfig();
            config.Type = node.Type;
            config.Params = node.Export();
            config.Child = new List<NodeConfig>();
            foreach (var child in node.GetChildNodes()) {
                config.Child.Add(ExportNode(child as SkillNode));
            }
            return config;
        }

        public static void ExportError(string errorCode) {
            EditorUtility.DisplayDialog("导出失败", errorCode, "确定");
        }
    }
}
