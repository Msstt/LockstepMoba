using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat.Actor {
    [CreateAssetMenu(fileName = "Minion", menuName = "Config/Minion")]
    public class MinionConfig : ActorConfig {
        [LabelText("蓝方预制体")]
        public string bluePrefabName;
        [LabelText("红方预制体")]
        public string redPrefabName;
        
        [LabelText("索敌距离")]
        public FloatF patrolDistance;
        [LabelText("追击距离")]
        public FloatF chaseDistance;
    }
}