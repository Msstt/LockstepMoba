using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Navmesh {
    public class NavmeshMapInfo : MonoBehaviour {
        [Header("Navmesh 网格数据")]
        public TextAsset surfaceData;
    }

    public class NavmeshSurface {
        public List<Vector3F> vertices;
        // 三个为一组，表示一个三角形，索引 vertices
        public List<int> indices;
    }
}