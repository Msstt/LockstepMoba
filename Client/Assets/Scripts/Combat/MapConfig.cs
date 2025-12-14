// 战斗相关一些位置配置，暂时先放这里

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat {
    public class MapConfig : MonoBehaviour {
        [LabelText("英雄生成点")]
        public List<Vector3F> spawnPoint;
    }
}