namespace Combat.Actor {
    public abstract partial class Actor {
        public void OnHit(HitInfo info) {
            Stats.Health -= info.damage.physical;
            
            if (Stats.Health <= FloatF.zero) {
                OnDead();
            }
        }

        public HitInfo CreateAttackHitInfo() {
            return new HitInfo {
                attacker = Uid,
                damage = new Damage {
                    physical = Stats.Attack,
                },
            };
        }
    }
}