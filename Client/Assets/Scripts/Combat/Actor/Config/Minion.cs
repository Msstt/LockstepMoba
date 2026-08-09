using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat.Actor {
    [CreateAssetMenu(fileName = "Minion", menuName = "Config/Minion")]
    public class MinionConfig : ScriptableObject {
        [LabelText("名称")]
        public string name;
        
        [LabelText("预制体")]
        public string prefabName;
        
        [LabelText("生命值")]
        public LevelNumber<int> health;
        
        [LabelText("攻击力")]
        public LevelNumber<int> attack;
        [LabelText("攻击速度")]
        public FloatF attackSpeed;
        [LabelText("攻击前摇百分比")]
        public FloatF attackWindupRatio;
        [LabelText("攻击距离")]
        public FloatF attackDistance;
        
        [LabelText("移速")]
        public FloatF moveSpeed;
        [LabelText("索敌距离")]
        public FloatF patrolDistance;
        [LabelText("追击距离")]
        public FloatF chaseDistance;
    }
}