// 技能树节点：等待指定时间

using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Skill.SkillNode {
    public class WaitForTime : Node {
        public class Param {
            [LabelText("等待时间")]
            public FloatF Time;
        }
        private Param param;
        private bool isAttackSpeed;
        private bool isWindup;
        
        public WaitForTime(bool isWindup) {
            isAttackSpeed = true;
            this.isWindup = isWindup;
        }

        public WaitForTime(JToken json) {
            isAttackSpeed = false;
            param = ParseParam<Param>(json);
        }

        protected override NodeState OnEnter(Context context) {
            Actor.Actor actor = GetActor(context);
            if (actor == null) {
                return NodeState.Fail;
            }
            FloatF ratio = Config.Champion[actor.Id].attackWindupRatio;
            FloatF time;
            if (isAttackSpeed) {
                if (isWindup) {
                    time = FloatF.one / actor.Stats.AttackSpeed * ratio;
                } else {
                    time = FloatF.one / actor.Stats.AttackSpeed * (1 - ratio);
                }
            } else {
                time = param.Time;
            }
            SetValue(context, "EndFrame", TimeUtils.GetFrame(time));
            return NodeState.Continue;   
        }

        protected override NodeState OnUpdate(Context context) {
            int endFrame = GetValueOrDefault(context, "EndFrame", -1);
            return GameMgr.Instance.Frame >= endFrame ? NodeState.Finish : NodeState.Continue;
        }
    }
}