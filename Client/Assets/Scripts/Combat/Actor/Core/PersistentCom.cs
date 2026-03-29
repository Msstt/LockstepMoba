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

        private bool isInited = false;
        
        public sealed override void Awake() {
            if (isInited) {
                isInited = true;
                Init();
            }
            ReLife();
        }

        protected virtual void Init() { }
        protected virtual void ReLife() { }

        public sealed override void Destroy() {
            Dead();
        }
        protected virtual void Dead() { }

        public sealed override void RenderUpdate() { }
    }
}