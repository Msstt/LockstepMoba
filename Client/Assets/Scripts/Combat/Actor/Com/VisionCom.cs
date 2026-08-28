using System;
using Combat.Fog;

namespace Combat.Actor {
    public class VisionCom : Com {
        private Action cancelHandleGlobal;
        private Action cancelHandleSelf;
        
        public override void Awake() {
            cancelHandleGlobal = FogUtils.AddVision(VisionType.Global, Actor.Pos, VisionRadius);
            if (ActorUtils.IsSameCamp(Actor.Uid)) {
                cancelHandleSelf = FogUtils.AddVision(VisionType.Self, Actor.Pos, VisionRadius);
            }
            
            Actor.Event.OnChangePos.Register(OnChangePos);
        }

        public override void Destroy() {
            Actor.Event.OnChangePos.UnRegister(OnChangePos);
        }

        private void OnChangePos(Vector3F pos) {
            cancelHandleGlobal?.Invoke();
            cancelHandleSelf?.Invoke();
            
            cancelHandleGlobal = FogUtils.AddVision(VisionType.Global, Actor.Pos, VisionRadius);
            if (ActorUtils.IsSameCamp(Actor.Uid)) {
                cancelHandleSelf = FogUtils.AddVision(VisionType.Self, Actor.Pos, VisionRadius);
            }
        }

        private FloatF VisionRadius {
            get {
                if (Actor.Type == ActorType.Champion) {
                    return Config.Vision.championVisionRadius;
                }
                if (Actor.Type == ActorType.Minion) {
                    return Config.Vision.minionVisionRadius;
                }
                if (Actor.Type == ActorType.Turret) {
                    return Config.Vision.buildingVisionRadius;
                }
                return FloatF.zero;
            }
        }
    }
}