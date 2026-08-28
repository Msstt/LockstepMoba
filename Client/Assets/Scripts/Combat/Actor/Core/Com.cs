namespace Combat.Actor {
    public abstract class Com : ICheckableData {
        private Actor actor = null;
        public Actor Actor {
            get => actor;
            set {
                if (actor != null) {
                    Log.Error($"{actor.Uid}'s Com is setting actor");
                    return;
                }
                actor = value;
            }
        }
        
        public virtual void Awake() { }
        
        // 同步帧的 Update
        public virtual void Update(int frame) { }
        
        // 表现层的 Update
        public virtual void RenderUpdate() { }
        
        public virtual void Destroy() { }

        public virtual int GetStatusCode() => Framework.StatusCode.Seed;
    }
}
