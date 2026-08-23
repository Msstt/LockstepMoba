using UnityEngine;
using GlobalConfig = Config;

namespace Combat.Actor {
    public class Minion : Actor {
        public int WaveIndex { get; private set; }
        
        public Minion(int id, int uid, GameObject go, CampType camp, int waveIndex) : base(id, uid, go, camp) {
            Type = ActorType.Minion;
            Config = GlobalConfig.Minion[id];
            WaveIndex = waveIndex;
        }
        
        public override void BindCom() {
            AddComponent<MoveCom>();
            AddComponent<AnimCom>();
            AddComponent<ControlCom>();
            
            AddComponent<BehaviourMachineCom>();
            AddComponent<BuffCom>();
            
            AddComponent<VisionCom>();
            
            AddComponent<NormalUICom>();
        }
    }
}