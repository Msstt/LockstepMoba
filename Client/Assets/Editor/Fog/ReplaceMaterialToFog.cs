using UnityEditor;
using UnityEngine;

namespace Editor.Fog {
    public static class ReplaceMaterialToFog {
        static readonly string[] GlobalProperties = { "_FogTex", "_FogStart", "_FogCellSize", "_FogSourceSize" };

        [MenuItem("工具/战争迷雾/替换地图材质")]
        public static void Execute() {
            Material fogMaterial = Resources.Load<Material>("Material/FogOfWar");
            if (fogMaterial == null) {
                Debug.LogError("未找到 FogOfWar 材质");
                return;
            }

            string savePath = "Assets/Resources/Material/FogInstances";
            if (!AssetDatabase.IsValidFolder(savePath)) {
                AssetDatabase.CreateFolder("Assets/Resources/Material", "FogInstances");
            }

            int count = 0;
            foreach (GameObject go in Selection.gameObjects) {
                foreach (Renderer renderer in go.GetComponentsInChildren<Renderer>(true)) {
                    Texture originalTex = renderer.sharedMaterial?.mainTexture;
                    Material instance = new Material(fogMaterial);
                    instance.name = fogMaterial.name + "_" + go.name;
                    if (originalTex != null) {
                        instance.SetTexture("_MainTex", originalTex);
                    }
                    string assetPath = AssetDatabase.GenerateUniqueAssetPath(savePath + "/" + instance.name + ".mat");
                    AssetDatabase.CreateAsset(instance, assetPath);
                    renderer.sharedMaterial = instance;
                    count++;
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"替换完成，共创建 {count} 个材质实例，保存至 {savePath}");
        }

        [MenuItem("工具/战争迷雾/同步材质属性")]
        public static void SyncMaterialProperties() {
            Material fogMaterial = Resources.Load<Material>("Material/FogOfWar");
            if (fogMaterial == null) {
                Debug.LogError("未找到 FogOfWar 材质");
                return;
            }

            Shader fogShader = Shader.Find("World/FogOfWarSprite");
            if (fogShader == null) {
                Debug.LogError("未找到 FogOfWar Shader");
                return;
            }

            string[] guids = AssetDatabase.FindAssets("t:Material");
            int count = 0;
            foreach (string guid in guids) {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (mat == null || mat.shader != fogShader || mat == fogMaterial) continue;

                for (int i = 0; i < fogMaterial.shader.GetPropertyCount(); i++) {
                    string propName = fogMaterial.shader.GetPropertyName(i);
                    if (propName == "_MainTex") continue;
                    int propId = Shader.PropertyToID(propName);
                    switch (fogMaterial.shader.GetPropertyType(i)) {
                        case UnityEngine.Rendering.ShaderPropertyType.Float:
                            mat.SetFloat(propId, fogMaterial.GetFloat(propId));
                            break;
                        case UnityEngine.Rendering.ShaderPropertyType.Range:
                            mat.SetFloat(propId, fogMaterial.GetFloat(propId));
                            break;
                        case UnityEngine.Rendering.ShaderPropertyType.Color:
                            mat.SetColor(propId, fogMaterial.GetColor(propId));
                            break;
                        case UnityEngine.Rendering.ShaderPropertyType.Vector:
                            mat.SetVector(propId, fogMaterial.GetVector(propId));
                            break;
                        case UnityEngine.Rendering.ShaderPropertyType.Texture:
                            mat.SetTexture(propId, fogMaterial.GetTexture(propId));
                            break;
                    }
                }

                EditorUtility.SetDirty(mat);
                count++;
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"同步完成，共处理 {count} 个 FogOfWar 材质实例");
        }
    }
}