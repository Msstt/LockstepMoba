using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Combat.Area {
    public class Area : IDisposable, ICheckableData {
        public Shape Shape { get; private set; }
        private List<IEffect> effects = new List<IEffect>();
        private List<IRaycast> raycasts = new List<IRaycast>();
        
        private List<List<Actor.Actor>> raycastResult = new List<List<Actor.Actor>>();
        
        public int ActorId { get; private set; }
        public int Id { get; private set; }
        public int Level { get; private set; }
        public int Uid { get; private set; }
        
        public GameObject GameObject { get; private set; }

        private float rawY = 0;

        // TODO 表现平滑
        private Vector3F position;
        public Vector3F Position {
            get => position;
            set {
                position = value;
                Vector3 rawPos = value.ToVector3();
                rawPos.y += rawY;
                GameObject.transform.position = rawPos;
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
        
        private int? targetUid;
        public int TargetUid => TargetUidIsValid ? targetUid.Value : throw new CombatException("Area TargetUid is not valid but accessed");
        public bool TargetUidIsValid => targetUid.HasValue;
        
        public Area(int areaId, int uid, int actorId, int level, Vector3F position, Vector3F direction, int? targetUid) {
            Id = areaId;
            ActorId = actorId;
            Level = level;
            Uid = uid;
            this.targetUid = targetUid;
            
            AreaConfig config = Config.Area[areaId];
            
            GameObject = new GameObject("Area_" + areaId);
            GameObject.transform.SetParent(AreaUtils.TransRoot);
            string prefabName = config.Prefab;
            if (prefabName == "{areaPrefabName}") {
                prefabName = ActorUtils.GetActor(actorId)?.Config.areaPrefabName;
            }
            var prefab = GoUtils.NewGo(prefabName, GameObject.transform);
            if (prefab != null) {
                prefab.name = "Prefab";
                rawY = prefab.transform.localPosition.y;
            }
            Position = position;
            Direction = direction;

            Shape = ShapeFactory.CreateShape(config.ShapeType, config.ShapeParam);
            
            foreach (EffectConfig effect in config.Effect) {
                effects.Add(EffectFactory.CreateEffect(this, effect));
            }

            foreach (RaycastConfig raycast in config.Raycast) {
                raycasts.Add(RaycastFactory.CreateEffect(this, raycast));
                raycastResult.Add(null);
            }
            
            ExecuteEffect((effect) => effect.OnCreate());
        }
        
        public void Dispose() {
            ExecuteEffect((effect) => effect.OnDestroy());
            effects.Clear();
            GameObject.Destroy(GameObject);
        }

        public void Update() {
            for (int i = 0; i < raycasts.Count; i++) {
                raycastResult[i] = null;
            }
            
            ExecuteEffect((effect) => effect.OnUpdate());

#if UNITY_EDITOR
            if (GameMgr.Instance.GMTool.ShowDebugMode) {
                Shape.RenderDebug(Position, Direction);
            }
#endif
        }
        
        private bool? lastVisibility = null;

        public void RenderUpdate() {
            ExecuteEffect((effect) => effect.OnRenderUpdate());
            
            bool nowVisibility = FogUtils.IsVisible(this);
            if (nowVisibility != lastVisibility) {
                GameObject.SetVisible(nowVisibility);
                lastVisibility = nowVisibility;
            }
        }
        
        private void ExecuteEffect(Action<IEffect> func) {
            foreach (IEffect effect in effects) {
                func(effect);
            }
        }

        public List<Actor.Actor> Raycast(int raycastId) {
            if (raycastId < 0 || raycastId >= raycasts.Count) {
                return new List<Actor.Actor>();
            }
            if (raycastResult[raycastId] == null) {
                raycastResult[raycastId] = raycasts[raycastId].Get();
            }
            return raycastResult[raycastId];
        }

        public int GetStatusCode() {
            int code = StatusCode.Seed;
            code = StatusCode.Combine(code, Id);
            code = StatusCode.Combine(code, Uid);
            code = StatusCode.Combine(code, ActorId);
            code = StatusCode.Combine(code, Level);
            code = StatusCode.Combine(code, Position);
            code = StatusCode.Combine(code, Direction);
            code = StatusCode.Combine(code, TargetUidIsValid);
            if (TargetUidIsValid) {
                code = StatusCode.Combine(code, targetUid.Value);
            }
            code = StatusCode.Combine(code, effects.Count);
            foreach (IEffect effect in effects) {
                code = StatusCode.CombineData(code, effect);
            }
            return code;
        }
    }
}
