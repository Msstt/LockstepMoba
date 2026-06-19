using System.Collections.Generic;
using Combat.Actor;
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
                List<int> units = NavmeshUtils.RaycastInCircle(typeBitSet, actor.Pos, param.Radius);
                lastInvisibility = units.Count > 0 ? 1 : 0;
                actor.Stats.Invisibility += lastInvisibility;
            }
        }
    }
}