// 技能树节点：播放动画

using Combat.Actor;
using Newtonsoft.Json.Linq;

namespace Combat.Skill.SkillNode {
    public class PlayAnimParam {
        public string AnimName;
    }
    
    public class PlayAnim : Node {
        private readonly string animName;
        
        public PlayAnim(JToken json) {
            PlayAnimParam param = ParseParam<PlayAnimParam>(json);
            animName = param.AnimName;
        }
        
        public override NodeState OnEnter(Context context) {
            AnimCom com = GetCom<AnimCom>(context);
            if (com == null) {
                return NodeState.Fail;
            }
            com.PlayAnim(animName);
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