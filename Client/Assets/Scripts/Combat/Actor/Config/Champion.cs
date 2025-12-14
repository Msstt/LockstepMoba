using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat.Actor {
    [CreateAssetMenu(fileName = "Champion", menuName = "Config/Champion")]
    public class ChampionConfig : ScriptableObject {
        [LabelText("名称")]
        public string name;
        
        [LabelText("预制体")]
        public string prefabName;
    }
}