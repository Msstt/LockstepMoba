using Combat.Actor;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OtherConfig {
    [CreateAssetMenu(fileName = "Time", menuName = "Config/Time")]
    public class Time : ScriptableObject {
        [LabelText("复活时间")]
        public LevelNumber<FloatF> championReviveTime;
    }
}