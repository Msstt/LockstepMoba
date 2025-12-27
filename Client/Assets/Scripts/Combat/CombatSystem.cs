using System.Collections.Generic;
using Codice.Client.BaseCommands;
using Combat.Actor;
using Framework;
using Navmesh;
using Network;
using UnityEngine;

// 处理对局相关

namespace Combat {
    public class CombatSystem : ICombatSystem {
        public Uid SelfUid { get; private set; }
        private List<Uid> playerUid = new List<Uid>();
        public IReadOnlyList<Uid> PlayerUid => playerUid;
        
        private Dictionary<Uid, int> championId = new Dictionary<Uid, int>();
        
        public MapConfig MapConfig { get; private set; }

        public void Init() {
            MapConfig = GameObject.Find("Map")?.GetComponent<MapConfig>();
            if (MapConfig == null) {
                Log.Error("MapConfig not found");
                return;
            }
        }
        
        public void Init(battle_start_s2c msg) {
            SelfUid = msg.SelfUid;
            playerUid.Clear();
            championId.Clear();
            foreach (var player in msg.Players) {
                playerUid.Add(player.Uid);
                championId[player.Uid] = player.ChampionId;
            }
            playerUid.Sort();
        }
        
        public int GetChampionId(Uid uid) {
            if (!championId.ContainsKey(uid)) {
                return -1;
            }
            return championId[uid];
        }
    }
}