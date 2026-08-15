using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OtherConfig {
    [CreateAssetMenu(fileName = "Map", menuName = "Config/Other/Map")]
    public class Map : ScriptableObject {
        [LabelText("复活位置")]
        public List<SimpleTransform> revivePos = new List<SimpleTransform>();
        
        [LabelText("蓝方兵线")]
        public List<Combat.Actor.MinionWave> blueMinionWavePos = new List<Combat.Actor.MinionWave>();
        [LabelText("红方兵线")]
        public List<Combat.Actor.MinionWave> redMinionWavePos = new List<Combat.Actor.MinionWave>();
    }
}