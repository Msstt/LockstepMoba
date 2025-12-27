using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Combat.Actor {
    public class ActorSystem : IActorSystem {
        public Transform TransRoot { get; private set; }

        private int maxUid = 0;
        
        private SortedDictionary<int, Actor> actors = new SortedDictionary<int, Actor>();
        public Champion SelfChampion { get; private set; }
        
        public void Init() {
            TransRoot = new GameObject("[Actor]").transform;
        }
        
        public void Start() {
            CreateChampion();
        }
        
        public void FrameUpdate(int frame) {
            foreach (var actor in actors.Values) {
                actor.Update(frame);
            }
        }

        public void Update() {
            foreach (var actor in actors.Values) {
                actor.RenderUpdate();
            }
        }
        
        private void CreateChampion() {
            ICombatSystem combat = GameMgr.Instance.GetSystem<ICombatSystem>();
            int index = 0;
            foreach (var uid in combat.PlayerUid) {
                var championId = combat.GetChampionId(uid);
                Champion actor = Champion.Create(championId);
                actor.SetPos(combat.MapConfig.spawnPoint[index++], true);
                actor.SetDir(new Vector3F( 1, 0, 0), true);
                actors[actor.Uid] = actor;
                if (uid == combat.SelfUid) {
                    SelfChampion = actor;
                }
            }
        }

        public int GetUid() {
            return ++maxUid;
        }
    }
}