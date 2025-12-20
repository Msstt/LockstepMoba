using System.Collections.Generic;
using Codice.Client.BaseCommands;
using Combat.Actor;
using Framework;
using Navmesh;
using Network;
using UnityEngine;

// 处理对局相关

namespace Combat {
    public class CombatMgr : Singleton<CombatMgr> {
        public bool IsRunning { get; private set; } = false;

        public Uid SelfUid { get; private set; }
        private List<Uid> playerUid = new List<Uid>();
        public IReadOnlyList<Uid> PlayerUid => playerUid;
        
        private Dictionary<Uid, int> championId = new Dictionary<Uid, int>();
        
        public MapConfig mapConfig { get; private set; }
        
        public void Start(battle_start_s2c msg) {
            IsRunning = true;
            mapConfig = GameObject.Find("Map")?.GetComponent<MapConfig>();
            if (mapConfig == null) {
                Log.Error("MapConfig not found");
                return;
            }
            
            SelfUid = msg.SelfUid;
            playerUid.Clear();
            championId.Clear();
            foreach (var player in msg.Players) {
                playerUid.Add(player.Uid);
                championId[player.Uid] = player.ChampionId;
            }
            playerUid.Sort();

            ActorMgr.Instance.Start();
            
            EventUtils.Send<EventType.OnBattleStart>();
        }

        public void Update() {
            ActorMgr.Instance.Update();
        }
        
        public int GetChampionId(Uid uid) {
            if (!championId.ContainsKey(uid)) {
                return -1;
            }
            return championId[uid];
        }
    }
}