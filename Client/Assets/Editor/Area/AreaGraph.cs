using System;
using System.Collections.Generic;
using System.Linq;
using Combat.Area;
using Framework;
using NodeCanvas.Framework;
using UnityEditor;
using UnityEngine;

namespace Editor.Area {
    [CreateAssetMenu(menuName = "Skill/SkillGraph")]
    public class AreaGraph : Graph {
        public static readonly string ImportRelaPath = "Assets/Resources/Config/Area/Node/";
        public static readonly string ImportPath = Application.dataPath + "/Resources/Config/Area/Node/";
        public static readonly string ExportPath = Application.dataPath + "/Resources/Config/Area/Json/";
        
        public override Type baseNodeType => typeof(AreaNode);
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
            
            AreaConfig config = root.config.Export();
            if (root.GetChildNodes().Any()) {
                config.Effect = new List<EffectConfig>();
                config.Raycast = new List<RaycastConfig>();
                foreach (var node in root.GetChildNodes()) {
                    if (TypeUtils.IsGenericType(node, typeof(EffectNode<>))) {
                        EffectConfig effect = new EffectConfig();
                        effect.Type = (node as AreaNode).EffectType;
                        effect.Params = (node as AreaNode).Export();
                        effect.RaycastId = -1;
                        config.Effect.Add(effect);
                    } else {
                        int raycastId = config.Raycast.Count;
                        RaycastConfig raycast = new RaycastConfig();
                        raycast.Type = (node as AreaNode).RaycastType;
                        raycast.Params = (node as AreaNode).Export();
                        config.Raycast.Add(raycast);
                        
                        foreach (var node2 in node.GetChildNodes()) {
                            if (TypeUtils.IsGenericType(node2, typeof(EffectNode<>))) {
                                EffectConfig effect = new EffectConfig();
                                effect.Type = (node2 as AreaNode).EffectType;
                                effect.Params = (node2 as AreaNode).Export();
                                effect.RaycastId = raycastId;
                                config.Effect.Add(effect);
                            }
                        }
                    }
                }
            }

            JsonHelper.SaveToFile(config, ExportPath + config.Id + ".json");
            AssetDatabase.Refresh();
            
            OnExportEnd?.Invoke();
            
            Debug.Log("区域导出成功: " + config.Id);
        }

        public static void ExportError(string errorCode) {
            EditorUtility.DisplayDialog("导出失败", errorCode, "确定");
        }
    }
}
