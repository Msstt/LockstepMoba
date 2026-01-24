using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat.Actor {
    [CreateAssetMenu(fileName = "Champion", menuName = "Config/Champion")]
    public class ChampionConfig : ScriptableObject {
        [LabelText("名称")]
        public string name;
        
        [LabelText("预制体")]
        public string prefabName;
        
        [LabelText("生命值")]
        public LevelNumber<int> health;
        
        [LabelText("攻击力")]
        public LevelNumber<int> attack;
        [LabelText("攻击速度")]
        public LevelNumber<FloatF> attackSpeed;
        [LabelText("攻击距离")]
        public FloatF attackDistance;
        
        [LabelText("移速")]
        public FloatF moveSpeed;
        
        [LabelText("技能")]
        public int[] skillIds = new int[SkillUtils.SkillSlotCount];
    }
}