using System.Collections.Generic;
using Combat.Actor;
using UnityEditor;
using UnityEngine;

namespace Editor {
    public static class ExportMapConfig {
        private const string MenuPath = "Assets/Config/导出地图配置";

        [MenuItem(MenuPath, true)]
        private static bool ValidateExecute() {
            return Selection.activeObject is OtherConfig.Map;
        }

        [MenuItem(MenuPath)]
        private static void Execute() {
            if (Selection.activeObject is not OtherConfig.Map mapConfig) {
                Debug.LogError("请先选中 OtherConfig.Map 配置资源");
                return;
            }

            Transform map = GameObject.Find("Map")?.transform;
            Transform config = map?.Find("Config");
            if (config == null) {
                Debug.LogError("当前场景中未找到 Map/Config 节点，未导出地图配置");
                return;
            }

            Transform reviveRoot = FindChild(config, "RevivePos", "Revive", "复活位置");
            Transform minionWaveRoot = FindChild(config, "MinionWavePos", "MinionWave", "兵线");
            Transform blueMinionWaveRoot = minionWaveRoot?.Find("Blue");
            Transform redMinionWaveRoot = minionWaveRoot?.Find("Red");
            Transform turretRoot = FindChild(config, "TurretPos", "Turret", "防御塔");
            Transform blueTurretRoot = turretRoot?.Find("Blue");
            Transform redTurretRoot = turretRoot?.Find("Red");

            List<SimpleTransform> revivePos = ExportTransforms(reviveRoot);
            if (revivePos.Count == 0) {
                Debug.LogError("Map/Config 下未找到复活点，未导出地图配置");
                return;
            }

            Undo.RecordObject(mapConfig, "导出地图配置");
            mapConfig.revivePos = revivePos;
            mapConfig.blueMinionWavePos = blueMinionWaveRoot == null ? new List<MinionWave>() : ExportMinionWaves(blueMinionWaveRoot);
            mapConfig.redMinionWavePos = redMinionWaveRoot == null ? new List<MinionWave>() : ExportMinionWaves(redMinionWaveRoot);
            mapConfig.blueTurretPos = ExportTransforms(blueTurretRoot);
            mapConfig.redTurretPos = ExportTransforms(redTurretRoot);
            EditorUtility.SetDirty(mapConfig);
            AssetDatabase.SaveAssets();

            Debug.Log($"地图配置导出完成：复活点 {mapConfig.revivePos.Count} 个，蓝方兵线 {mapConfig.blueMinionWavePos.Count} 条，红方兵线 {mapConfig.redMinionWavePos.Count} 条，蓝方防御塔 {mapConfig.blueTurretPos.Count} 个，红方防御塔 {mapConfig.redTurretPos.Count} 个");
        }

        private static Transform FindChild(Transform parent, params string[] names) {
            foreach (string name in names) {
                Transform child = parent.Find(name);
                if (child != null) {
                    return child;
                }
            }
            return null;
        }

        private static List<SimpleTransform> ExportTransforms(Transform root, Transform excludedChild = null) {
            List<SimpleTransform> result = new List<SimpleTransform>();
            if (root == null) {
                return result;
            }

            for (int i = 0; i < root.childCount; i++) {
                Transform child = root.GetChild(i);
                if (child == excludedChild) {
                    continue;
                }
                result.Add(ToSimpleTransform(child));
            }
            return result;
        }

        private static List<MinionWave> ExportMinionWaves(Transform root) {
            List<MinionWave> result = new List<MinionWave>();
            for (int laneIndex = 0; laneIndex < root.childCount; laneIndex++) {
                MinionWave lanes = new MinionWave {
                    Pos = ExportTransforms(root.GetChild(laneIndex))
                };
                result.Add(lanes);
            }
            return result;
        }

        private static SimpleTransform ToSimpleTransform(Transform transform) {
            Vector3 forward = transform.forward;
            return new SimpleTransform {
                position = transform.position.ToVector3F(),
                direction = Mathf.Atan2(forward.z, forward.x).ToFloatF(),
            };
        }
    }
}
