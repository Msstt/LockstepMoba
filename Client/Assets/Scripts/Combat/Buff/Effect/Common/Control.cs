using System;
using Combat.Actor;
using Combat.Skill;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Buff.Effect {
    public class Control : Effect<Control.Param> {
        public class Param {
            [LabelText("技能类型")]
            public SkillType Type;
        }
        
        public Control(Buff buff, JToken json) : base(buff, json) { }

        private Action releaseFunc;

        public override void OnCreate() {
            ControlCom com = ActorUtils.GetCom<ControlCom>(buff.ActorId);
            releaseFunc = com?.Abort(param.Type);
        }

        public override void OnDestroy() {
            releaseFunc?.Invoke();
        }
    }
}