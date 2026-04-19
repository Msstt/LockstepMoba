using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Navmesh {
    public class NavmeshMapInfo : MonoBehaviour {
        [LabelText("Navmesh 网格数据")]
        public TextAsset surfaceData;
        [LabelText("Navmesh 高度数据")]
        public TextAsset heightMap;
        [LabelText("视野遮罩数据")]
        public TextAsset visionBlockerMap;
        
        [FoldoutGroup("调试参数")]
        [LabelText("绘制网格三角形")]
        public bool DrawDebugTriangle;
        [FoldoutGroup("调试参数")]
        [LabelText("绘制网格连接")]
        public bool DrawDebugConnection;
    }

    public class NavmeshSurface {
        public List<Vector3F> vertices;
        // 三个为一组，表示一个三角形，索引 vertices
        public List<int> indices;

        public (Vector3F, Vector3F) GetBorder() {
            Vector3F min = vertices[0], max = vertices[0];
            for (int i = 0; i < vertices.Count; i++) {
                min.x = FloatF.Min(min.x, vertices[i].x);
                min.y = FloatF.Min(min.y, vertices[i].y);
                min.z = FloatF.Min(min.z, vertices[i].z);
                max.x = FloatF.Max(max.x, vertices[i].x);
                max.y = FloatF.Max(max.y, vertices[i].y);
                max.z = FloatF.Max(max.z, vertices[i].z);
            }
            return (min, max);
        }
    }
}