using System.Collections.Generic;
using Network;

namespace Combat {
    public interface ICombatSystem : IInitSystem {
        
        public Uid SelfUid { get; }
        public IReadOnlyList<Uid> PlayerUid { get; }
        public MapConfig MapConfig { get; }

        public void Init(battle_start_s2c msg);
        public int GetChampionId(Uid uid);
    }
}