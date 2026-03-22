using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OtherConfig {
    [CreateAssetMenu(fileName = "Map", menuName = "Config/Map")]
    public class Map : ScriptableObject {
        [LabelText("复活位置")]
        public List<SimpleTransform> revivePos = new List<SimpleTransform>();
    }
}