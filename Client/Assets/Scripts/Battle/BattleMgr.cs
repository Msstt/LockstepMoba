using Framework;
using Network;

// 处理对局相关

namespace Battle {
    public class BattleMgr : Singleton<BattleMgr> {
        public bool IsRunning { get; private set; } = false;

        public Uid SelfUid { get; private set; }
        
        public void Start(battle_start_s2c msg) {
        }
    }
}