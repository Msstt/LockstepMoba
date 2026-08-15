using System.Collections.Generic;
using Combat.Actor;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OtherConfig {
    [CreateAssetMenu(fileName = "Exp", menuName = "Config/Other/Exp")]
    public class Exp : ScriptableObject {
        [LabelText("升级经验(等级)")]
        public LevelNumber<int> upgradeExp = new LevelNumber<int>();
        [LabelText("英雄击杀经验(等级)")]
        public LevelNumber<int> killChampionExp = new LevelNumber<int>();
        
        [LabelText("通用升级大招等级")]
        public List<int> skillRRequestLevel = new List<int>();
    }
}