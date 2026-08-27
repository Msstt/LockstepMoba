using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat.Actor {
    public class ActorConfig : ScriptableObject {
        [LabelText("名称")]
        public string name;
        
        [LabelText("生命值")]
        public LevelNumber<int> health;
        
        [LabelText("攻击力")]
        public LevelNumber<int> attack;
        [LabelText("攻击速度")]
        public LevelNumber<FloatF> attackSpeed;
        [LabelText("攻击前摇百分比")]
        public FloatF attackWindupRatio;
        [LabelText("攻击距离")]
        public FloatF attackDistance;
        
        [LabelText("普通攻击预制体")]
        public string areaPrefabName;
        [LabelText("普通攻击移动速度")]
        public FloatF areaVelocity;
        
        [LabelText("移速")]
        public FloatF moveSpeed;
        
        [LabelText("半径")]
        public FloatF radius;
    }
}