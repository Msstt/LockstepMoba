namespace Combat.Actor {
    public abstract partial class Actor {
        public void OnHit(HitInfo info) {
            Stats.Health -= info.damage.physical;
            
            if (Stats.Health.Value <= 0) {
                OnDead();
            }
        }
    }
}