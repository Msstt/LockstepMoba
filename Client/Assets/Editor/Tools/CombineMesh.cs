using UnityEditor;
using UnityEngine;

namespace Editor {
    public class CombineMesh {
        [MenuItem("工具/模型/合并网格")]
        public static void Execute() {
            GameObject go = Selection.activeGameObject;
            if (go == null) {
                Debug.LogError("未选择任何物体");
                return;
            }
            MeshRenderer[] meshRenders = go.GetComponentsInChildren<MeshRenderer>();
 
            Material[] mats = new Material[meshRenders.Length];
            for ( int i = 0; i < meshRenders.Length; i++ ) {
                mats[i] = meshRenders[i].sharedMaterial;
            }
 
            MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>();
 
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            for ( int i = 0; i < meshFilters.Length; i++ ) {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = go.transform.worldToLocalMatrix * meshFilters[i].transform.localToWorldMatrix;
            }
 
            MeshRenderer mr = go.EnsureComponent<MeshRenderer>();
            MeshFilter mf = go.EnsureComponent<MeshFilter>();
            mf.mesh = new Mesh();
            mf.mesh.CombineMeshes( combine, false );
            mr.sharedMaterials = mats;
            Debug.Log("合并完成");
        }
    }
}