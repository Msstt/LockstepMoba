using Combat.Actor;
using Combat.Skill;

namespace EventType {
    public struct OnConnected { }
    
    public struct OnLockStepStart { }
    public struct OnGameStart { }
    
    public struct ActorDead {
        public int Uid;
        public ActorType Type;
        public int KillerUid;
    }
    
    public struct ChampionLevelUp {
        public int Uid;
        public int Level;
    }
    
    public struct ChampionSkillLevelUp {
        public int Uid;
        public SkillSlot Slot;
        public int Level;
    }
}