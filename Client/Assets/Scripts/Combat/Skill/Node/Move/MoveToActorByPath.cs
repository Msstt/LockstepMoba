// 技能树节点：按路径移动到目标单位

using Combat.Actor;
using Newtonsoft.Json.Linq;

namespace Combat.Skill.SkillNode {
    public class MoveToActorByPath : MoveNode {
        public class Param {
            public FloatF Distance;
        }
        private readonly Param param;
        private readonly bool inAttackDistance;
        
        public MoveToActorByPath() {
            inAttackDistance = true;
        }
        
        public MoveToActorByPath(JToken json) {
            inAttackDistance = false;
            param = ParseParam<Param>(json);
        }
        
        protected override NodeState Move(Context context, MoveCom com) {
            if (!context.Param.UidIsValid) {
                return NodeState.Fail;
            }
            Stats stats = GetStats(context);
            if (stats == null) {
                return NodeState.Fail;
            }
            
            FloatF distance = inAttackDistance ? stats.AttackDistance : param.Distance;
            com.MoveToActorByPath(context.Param.Uid, distance, () => {
                MoveFinish(context);
            }, () => {
                MoveFail(context);
            });
            return NodeState.Continue;
        }
    }
}