namespace Combat.Actor {
    public struct Damage {
        public static readonly Damage zero = new Damage {
            physical = FloatF.zero,
            magic = FloatF.zero,
            @true = FloatF.zero,
        };
        
        public FloatF physical;  // 物理伤害
        public FloatF magic;  // 魔法伤害
        public FloatF @true;  // 真实伤害
    }
    
    public struct HitInfo {
        public int attacker;
        public Damage damage;
    }
}