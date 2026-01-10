namespace Combat.Actor {
    public abstract partial class Actor {
        public void OnHit(HitInfo info) {
            Stats.Health -= info.damage.physical;
            
            Log.Info(Stats.Health.Value.ToString());
            
            if (Stats.Health.Value <= 0) {
                OnDead();
            }
        }
    }
}