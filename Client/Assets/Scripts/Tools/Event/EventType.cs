namespace EventType {
    public struct OnConnected { }
    
    public struct OnLockStepStart { }

    public struct ActorDead {
        public int Uid;
        public int KillerUid;
    }
}