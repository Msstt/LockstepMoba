using System;
using Combat.Actor;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Skill.SkillNode {
    public class RequestSlot : ParamNode<RequestSlot.Param> {
        public class Param {
            [DrawWithUnity]
            [LabelText("槽位")]
            public SlotCom.Slot Slot;
            [LabelText("等待时间")]
            public FloatF WaitTime;
        }
        public RequestSlot(JToken json) : base(json) { }
        
        protected override NodeState OnEnter(Context context) {
            SlotCom com = GetCom<SlotCom>(context);
            if (com == null) {
                return NodeState.Fail;
            }
            
            SetValue(context, "Res", -1);
            long requestId = com.RequestInTime((int)param.Slot, param.WaitTime, (ReleaseFunc) => {
                SetValue(context, "ReleaseFunc", ReleaseFunc);
                SetValue(context, "Res", 0);
            }, () => {
                SetValue(context, "Res", 1);
            });
            SetValue(context, "RequestId", requestId);
            return NodeState.Continue;
        }

        protected override NodeState OnUpdate(Context context) {
            int res = GetValueOrDefault<int>(context, "Res", -1);
            switch (res) {
                case -1:
                    return NodeState.Continue;
                case 0:
                    return NodeState.Finish;
                case 1:
                    return NodeState.Fail;
                default:
                    return NodeState.NoKnow;
            }
        }
        
        protected override void OnFinish(Context context) {
            var releaseFunc = GetValue<Action>(context, "ReleaseFunc");
            releaseFunc?.Invoke();
        }

        protected override void OnFail(Context context) {
            int res = GetValue<int>(context, "Res");
            long requestId = GetValue<long>(context, "RequestId");
            if (res == -1) {
                // 还在等待，取消请求
                GetCom<SlotCom>(context)?.Cancel(requestId);
            } else if (res == 0) {
                // 已经获得槽位，释放槽位
                var releaseFunc = GetValue<Action>(context, "ReleaseFunc");
                releaseFunc?.Invoke();
            }
        }
    }
}