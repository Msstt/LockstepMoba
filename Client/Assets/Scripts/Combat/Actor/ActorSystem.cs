using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Combat.Actor {
    public class ActorSystem : IActorSystem {
        public Transform TransRoot { get; private set; }

        private int maxUid = 0;
        
        private SortedDictionary<int, Actor> actors = new SortedDictionary<int, Actor>();
        
        public void Start() {
            TransRoot = new GameObject("[Actor]").transform;
        }
        
        public void FrameStart() {
            CreateChampion();
        }
        
        public void FrameUpdate() {
            foreach (var actor in actors.Values) {
                actor.Update();
            }
        }
        
        public void Update() { }
        
        private void CreateChampion() {
            ICombatSystem combat = GameMgr.Instance.GetSystem<ICombatSystem>();
            int index = 0;
            foreach (var uid in combat.PlayerUid) {
                var championId = combat.GetChampionId(uid);
                Champion actor = Champion.Create(championId);
                actor.Pos = combat.MapConfig.spawnPoint[index++];
                actors[actor.Uid] = actor;
            }
        }

        public int GetUid() {
            return ++maxUid;
        }
    }
}