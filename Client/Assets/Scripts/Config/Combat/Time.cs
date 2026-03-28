using Combat.Actor;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OtherConfig {
    [CreateAssetMenu(fileName = "Time", menuName = "Config/Time")]
    public class Time : ScriptableObject {
        [LabelText("复活时间(等级)")]
        public LevelNumber<FloatF> championReviveTime;
    }
}