using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat.Equipment {
    [CreateAssetMenu(fileName = "Equipment", menuName = "Config/Equipment")]
    public class EquipmentConfig : ScriptableObject {
        [LabelText("名称")]
        public string name;
        
        [LabelText("主动技能")]
        public int skillId;
        [LabelText("Buff")]
        public List<int> buffId;
    }
}