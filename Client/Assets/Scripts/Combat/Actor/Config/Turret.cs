using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat.Actor {
    [CreateAssetMenu(fileName = "Turret", menuName = "Config/Turret")]
    public class TurretConfig : ActorConfig {
        [LabelText("蓝方预制体")]
        public string bluePrefabName;
        [LabelText("红方预制体")]
        public string redPrefabName;
    }
}