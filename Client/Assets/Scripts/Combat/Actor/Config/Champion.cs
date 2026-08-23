using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat.Actor {
    [CreateAssetMenu(fileName = "Champion", menuName = "Config/Champion")]
    public class ChampionConfig : ActorConfig {
        [LabelText("预制体")]
        public string prefabName;
        
        [LabelText("技能")]
        public SkillInfo[] skill = new SkillInfo[6];
    }

    [Serializable]
    public class SkillInfo {
        [LabelText("技能ID")]
        public int skillId;
        [LabelText("初始等级")]
        public int initLevel;
        [LabelText("最大等级")]
        public int maxLevel;
    }
}