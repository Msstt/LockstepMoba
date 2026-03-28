namespace Combat.Actor {
    public interface ISpawnSystem : ISystem, IStartSystem, IFrameUpdateSystem {
        public void ReviveChampion(int uid);
    }
}