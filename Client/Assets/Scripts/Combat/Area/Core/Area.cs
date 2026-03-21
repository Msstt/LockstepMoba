using System;
using System.Collections.Generic;
using Codice.Client.Commands.WkTree;
using UnityEngine;

namespace Combat.Area {
    public class Area : IDisposable {
        public Shape Shape { get; private set; }
        private List<IEffect> effects = new List<IEffect>();
        
        public int ActorId { get; private set; }
        public int Level { get; private set; }
        
        public GameObject GameObject { get; private set; }

        // TODO 表现平滑
        private Vector3F position;
        public Vector3F Position {
            get => position;
            set {
                position = value;
                GameObject.transform.position = value.ToVector3();
            }
        }
        
        private Vector3F direction;
        public Vector3F Direction {
            get => direction;
            set {
                direction = value;
                GameObject.transform.forward = value.ToVector3();
            }
        }
        
        public Area(int areaId, int actorId, int level, Vector3F position, Vector3F direction) {
            ActorId = actorId;
            Level = level;
            
            AreaConfig config = Config.Area[areaId];
            
            GameObject = new GameObject("Area_" + areaId);
            GameObject.transform.SetParent(AreaUtils.TransRoot);
            var prefab = GoUtils.NewGo(config.Prefab, GameObject.transform, true);
            if (prefab != null) {
                prefab.name = "Prefab";
            }
            Position = position;
            Direction = direction;

            Shape = ShapeFactory.CreateShape(config.ShapeType, config.ShapeParam);
            
            foreach (EffectConfig effect in config.Effect) {
                effects.Add(EffectFactory.CreateEffect(this, effect));
            }
            
            ExecuteEffect((effect) => effect.OnCreate());
        }
        
        public void Dispose() {
            effects.Clear();
            ExecuteEffect((effect) => effect.OnDestroy());
        }

        public void Update() {
            ExecuteEffect((effect) => effect.OnUpdate());
        }

        public void RenderUpdate() {
            ExecuteEffect((effect) => effect.OnRenderUpdate());
        }
        
        private void ExecuteEffect(Action<IEffect> func) {
            foreach (IEffect effect in effects) {
                func(effect);
            }
        }
    }
}