using System.Collections.Generic;
using Combat.Actor;
using Network;
using UnityEngine;

// 处理对局相关

namespace Combat {
    public class CombatSystem : ICombatSystem {
        public Uid SelfUid { get; private set; }
        private List<Uid> playerUid = new List<Uid>();
        public IReadOnlyList<Uid> PlayerUid => playerUid;
        
        private Dictionary<Uid, battle_start_s2c.Types.player_info> playerInfo = new Dictionary<Uid, battle_start_s2c.Types.player_info>();

        public void Init() {
        }
        
        public void SetStartInfo(battle_start_s2c msg) {
            SelfUid = msg.SelfUid;
            playerUid.Clear();
            playerInfo.Clear();
            foreach (var player in msg.Players) {
                playerUid.Add(player.Uid);
                playerInfo[player.Uid] = player;
            }
            playerUid.Sort();
        }
        
        public int GetChampionId(Uid uid) {
            return playerInfo.TryGetValue(uid, out var info) ? info.ChampionId : -1;
        }

        public CampType GetCamp(Uid uid) {
            return playerInfo.TryGetValue(uid, out var info) ? (CampType)info.Camp : CampType.UnKnown;
        }
        
        public (int, int) GetSummonerSkill(Uid uid) {
            if (playerInfo.TryGetValue(uid, out var info) && info.Skill.Count >= 2) {
                return (info.Skill[0], info.Skill[1]);
            }
            return (-1, -1);
        }
    }
}