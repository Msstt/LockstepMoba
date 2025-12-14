using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Navmesh {
    public class NavmeshMapInfo : MonoBehaviour {
        [LabelText("Navmesh 网格数据")]
        public TextAsset surfaceData;
        
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
    }
}