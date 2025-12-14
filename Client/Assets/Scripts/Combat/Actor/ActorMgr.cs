using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Combat.Actor {
    public class ActorMgr : Singleton<ActorMgr> {
        public Transform TransRoot { get; private set; }

        private int maxUid = 0;
        
        private Dictionary<int, Actor> actors = new Dictionary<int, Actor>();
        
        public void Start() {
            TransRoot = new GameObject("[Actor]").transform;

            CreateChampion();
        }
        
        public void Update() {
            foreach (var actor in actors.Values) {
                actor.Update();
            }
        }
        
        private void CreateChampion() {
            int index = 0;
            foreach (var uid in CombatMgr.Instance.PlayerUid) {
                var championId = CombatMgr.Instance.GetChampionId(uid);
                Champion actor = Champion.Create(championId);
                actor.Pos = CombatMgr.instance.mapConfig.spawnPoint[index++];
                actors[actor.Uid] = actor;
            }
        }

        public int GetUid() {
            return ++maxUid;
        }
    }
}