// 技能树节点：播放动画

using Combat.Actor;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Skill.SkillNode {
    public class PlayAnim : Node {
        [System.Serializable]
        public class Param {
            [LabelText("动画名")]
            public string AnimName;
        }
        private readonly Param param;
        
        public PlayAnim(JToken json) {
            param = ParseParam<Param>(json);
        }
        
        protected override NodeState OnEnter(Context context) {
            AnimCom com = GetCom<AnimCom>(context);
            if (com == null) {
                return NodeState.Fail;
            }
            
            com.PlayAnim(param.AnimName);
            return NodeState.Finish;
        }
        
        protected override NodeState OnUpdate(Context context) => NodeState.Finish;
        
        protected override void OnFinish(Context context) {
            GetCom<AnimCom>(context).PlayAnim("Idle");
        }

        protected override void OnFail(Context context) {
            GetCom<AnimCom>(context).PlayAnim("Idle");
        }
    }
}