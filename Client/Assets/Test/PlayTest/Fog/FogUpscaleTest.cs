using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class FogUpscaleTest {
    private const string MaterialPath = "Assets/Test/Scenes/FogTest/FogUpscaleTest.mat";

    [SetUp]
    public void Setup() {
        SceneManager.LoadScene("FogUpscaleTest", LoadSceneMode.Single);
    }

    [UnityTest]
    public IEnumerator Test() {
        GameObject list = GameObject.Find("List");
        Assert.IsNotNull(list);

        SpriteRenderer[] renderers = list.GetComponentsInChildren<SpriteRenderer>();
        Assert.IsNotEmpty(renderers);

        Material[] originalMaterials = new Material[renderers.Length];
        MaterialPropertyBlock[] originalPropertyBlocks = new MaterialPropertyBlock[renderers.Length];
        MaterialPropertyBlock[] upscalePropertyBlocks = new MaterialPropertyBlock[renderers.Length];
        for (int i = 0; i < renderers.Length; i++) {
            originalMaterials[i] = renderers[i].sharedMaterial;

            originalPropertyBlocks[i] = new MaterialPropertyBlock();
            renderers[i].GetPropertyBlock(originalPropertyBlocks[i]);

            upscalePropertyBlocks[i] = new MaterialPropertyBlock();
            renderers[i].GetPropertyBlock(upscalePropertyBlocks[i]);
            upscalePropertyBlocks[i].SetTexture("_MainTex", renderers[i].sprite.texture);
        }

#if UNITY_EDITOR
        Material upscaleMaterial = AssetDatabase.LoadAssetAtPath<Material>(MaterialPath);
#else
        Material upscaleMaterial = new Material(Shader.Find("World/FogOfWarUpscale"));
#endif
        Assert.IsNotNull(upscaleMaterial);

        bool useUpscaleMaterial = false;
        while (true) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                useUpscaleMaterial = !useUpscaleMaterial;
                for (int i = 0; i < renderers.Length; i++) {
                    renderers[i].sharedMaterial =
                        useUpscaleMaterial ? upscaleMaterial : originalMaterials[i];
                    renderers[i].SetPropertyBlock(
                        useUpscaleMaterial
                            ? upscalePropertyBlocks[i]
                            : originalPropertyBlocks[i]
                    );
                }
            }

            yield return null;
        }
    }
}
