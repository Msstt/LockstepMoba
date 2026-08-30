using Combat.Actor;
using Framework;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Buff.Effect {
    public class InvisibilityByRadius : Effect<InvisibilityByRadius.Param> {
        // 破隐类型
        private const int typeBitSet = (int)(ActorType.Champion | ActorType.Minion);
        
        public class Param {
            [LabelText("潜行半径")]
            public FloatF Radius;
        }
        
        public InvisibilityByRadius(Buff buff, JToken json) : base(buff, json) { }

        private int lastInvisibility = 0;
        
        // TODO 加载潜行 UI

        public override void OnUpdate() {
            Actor.Actor actor = ActorUtils.GetActor(buff.ActorId);
            if (actor != null) {
                actor.Stats.Invisibility -= lastInvisibility;
                using (PooledList<int> units = PooledList<int>.Get()) {
                    NavmeshUtils.RaycastInCircle(typeBitSet, actor.Pos, param.Radius, units);
                    lastInvisibility = 1;
                    foreach (int uid in units) {
                        if (!ActorUtils.IsSameCamp(buff.ActorId, uid)) {
                            lastInvisibility = 0;
                            break;
                        }
                    }
                }
                actor.Stats.Invisibility += lastInvisibility;
            }
        }

        public override void OnDestroy() {
            Actor.Actor actor = ActorUtils.GetActor(buff.ActorId);
            if (actor != null) {
                actor.Stats.Invisibility -= lastInvisibility;
            }
        }
    }
}
