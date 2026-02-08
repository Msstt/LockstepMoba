using System;
using System.Collections.Generic;
using System.Linq;
using Combat.Buff;
using Framework;
using NodeCanvas.Framework;
using UnityEditor;
using UnityEngine;

namespace Editor.Buff {
    [CreateAssetMenu(menuName = "Skill/SkillGraph")]
    public class BuffGraph : Graph {
        public static readonly string ImportRelaPath = "Assets/Resources/Config/Buff/Node/";
        public static readonly string ImportPath = Application.dataPath + "/Resources/Config/Buff/Node/";
        public static readonly string ExportPath = Application.dataPath + "/Resources/Config/Buff/Json/";
        
        public override Type baseNodeType => typeof(BuffNode);
        public override bool requiresAgent => false;
        public override bool requiresPrimeNode => false;
        public override bool isTree => true;
        public override bool allowBlackboardOverrides => false;
        public override bool canAcceptVariableDrops => true;
        
        public Action OnExportEnd;

        protected override void OnGraphEditorToolbar() {
            if (GUILayout.Button("Export")) {
                Export();
            }
        }
        
        public void Export(bool noCheckNode = false) {
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
            
            BuffConfig config = root.config.Export();
            if (root.GetChildNodes().Any()) {
                config.Effect = new List<EffectConfig>();
                foreach (var node in root.GetChildNodes()) {
                    EffectConfig effect = new EffectConfig();
                    effect.Type = (node as BuffNode).Type;
                    effect.Params = (node as BuffNode).Export();
                    config.Effect.Add(effect);
                }
            }

            JsonHelper.SaveToFile(config, ExportPath + config.Id + ".json");
            AssetDatabase.Refresh();
            
            OnExportEnd?.Invoke();
            
            Debug.Log("Buff导出成功: " + config.Id);
        }

        public static void ExportError(string errorCode) {
            EditorUtility.DisplayDialog("导出失败", errorCode, "确定");
        }
    }
}
