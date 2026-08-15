using Sirenix.OdinInspector;
using UnityEngine;

namespace OtherConfig {
    [CreateAssetMenu(fileName = "Vision", menuName = "Config/Other/Vision")]
    public class Vision : ScriptableObject {
        [LabelText("英雄视野半径")]
        public FloatF championVisionRadius;
    }
}