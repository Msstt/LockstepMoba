// 技能树节点：播放动画

using Combat.Actor;
using Newtonsoft.Json.Linq;

namespace Combat.Skill.SkillNode {
    public class PlayAnim : Node {
        [System.Serializable]
        public class Param {
            public string AnimName;
        }
        private readonly Param param;
        
        public PlayAnim(JToken json) {
            param = ParseParam<Param>(json);
        }
        
        public override NodeState OnEnter(Context context) {
            AnimCom com = GetCom<AnimCom>(context);
            if (com == null) {
                return NodeState.Fail;
            }
            
            com.PlayAnim(param.AnimName);
            return NodeState.Finish;
        }
        
        public override NodeState OnUpdate(Context context) => NodeState.Finish;
        
        public override void OnFinish(Context context) {
            GetCom<AnimCom>(context).PlayAnim("Idle");
        }

        public override void OnFail(Context context) {
            GetCom<AnimCom>(context).PlayAnim("Idle");
        }
    }
}