namespace Combat.Actor {
    public abstract partial class Actor {
        public void OnHeal(FloatF heal) {
            Stats.Health += heal;
        }
    }
}