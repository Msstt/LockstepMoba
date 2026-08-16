namespace Combat.Actor {
    public abstract partial class Actor {
        public void OnHit(HitInfo info) {
            Stats.Health -= info.damage.physical;
            Stats.Health -= info.damage.magic;
            Stats.Health -= info.damage.@true;
            
            if (Stats.Health <= FloatF.zero) {
                OnDead();
                EventUtils.Send(new EventType.ActorDead {
                    Uid = Uid,
                    Type = Type,
                    KillerUid = info.attacker,
                });
            }
            
            Event.OnHit.Send(info.damage);
        }
    }
}