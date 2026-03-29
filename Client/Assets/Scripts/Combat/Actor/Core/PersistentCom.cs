// 自动持久化的组件，所有逻辑都是无状态的，可在死亡后继续访问

namespace Combat.Actor {
    public abstract class PersistentCom : Com {
        public new Actor Actor {
            get => throw new CombatException("PersistentCom's Actor cannot be get");
            set => throw new CombatException("PersistentCom's Actor cannot be set");
        }
        
        private int? uid = null;
        public int Uid {
            get => uid.Value;
            set {
                if (uid != null) {
                    Log.Error($"{uid.Value}'s PersistentCom is setting uid");
                    return;
                }
                uid = value;
            }
        }
        
        public sealed override void Awake() {
            ReLife();
        }
        public virtual void ReLife() { }

        public sealed override void Destroy() {
            Dead();
        }
        public virtual void Dead() { }
    }
}