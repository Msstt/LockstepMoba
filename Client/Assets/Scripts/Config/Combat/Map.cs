using System.Collections.Generic;
using Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OtherConfig {
    [CreateAssetMenu(fileName = "Map", menuName = "Config/Map")]
    public class Map : ScriptableObject {
        [LabelText("复活位置")]
        public List<SimpleTransform> revivePos = new List<SimpleTransform>();
        
        [LabelText("蓝方兵线")]
        public List<MinionWave> blueMinionWavePos = new List<MinionWave>();
        [LabelText("红方兵线")]
        public List<MinionWave> redMinionWavePos = new List<MinionWave>();
    }
}