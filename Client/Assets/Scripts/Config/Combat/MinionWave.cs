using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OtherConfig {
    [CreateAssetMenu(fileName = "MinionWave", menuName = "Config/Other/MinionWave")]
    public class MinionWave : ScriptableObject {
        [LabelText("开始时间")]
        public FloatF spawnStartTime;
        
        [LabelText("波次间隔时间")]
        public FloatF spawnIntervalTime;
        
        [LabelText("单位间隔时间")]
        public FloatF singleIntervalTime;

        [LabelText("单位ID")]
        public List<SerializableList<int>> spawnId = new List<SerializableList<int>>();
    }
}