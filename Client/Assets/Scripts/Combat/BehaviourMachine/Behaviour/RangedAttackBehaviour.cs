using Combat.Actor;
using Combat.Skill;

namespace Combat.BehaviourMachine {
    public class RangedAttackBehaviour : ActorBehaviour {
        public override SkillType SkillType => SkillType.NormalAttack;
        
        private int attackWindupFrame;
        private int attackFrame;
        private bool waitStart;
        private bool waitAttack;
        private Actor.Actor target;
        private int startFrame;

        public RangedAttackBehaviour(Machine machine, FloatF attackWindupRatio) : base(machine) {
            attackWindupFrame = TimeUtils.GetFrameCount(attackWindupRatio * FloatF.one/ Actor.Stats.AttackSpeed);
            attackFrame = TimeUtils.GetFrameCount(FloatF.one/ Actor.Stats.AttackSpeed);
        }
        
        public override bool Evaluate() {
            if (target != null) {
                return true;
            }
            
            foreach (var uid in NavmeshUtils.RaycastInCircle(Actor.Pos, Actor.Stats.AttackDistance)) {
                Actor.Actor actor = ActorUtils.GetActor(uid);
                if (actor != null && actor.Camp != Actor.Camp) {
                    StartAttack(actor);
                    break;
                }
            }

            return target != null;
        }

        public override void Execute(int frame) {
            if (waitStart) {
                waitStart = false;
                Actor.GetComponent<AnimCom>()?.PlayAnim("Attack");
            }
            
            if (waitAttack && frame >= startFrame + attackWindupFrame) {
                waitAttack = false;
                Attack();
            }
            
            if (frame >= startFrame + attackFrame) {
                StopAttack();
            }
        }

        public override void OnAbort() {
            StopAttack();
        }

        private void StartAttack(Actor.Actor target) {
            this.target = target;
            startFrame = GameMgr.Instance.Frame;
            waitStart = waitAttack = true;
        }

        private void Attack() {
            if (target == null) {
                return;
            }
            // 重新获取 Actor，可能前摇时 Actor 死亡
            Actor.Actor actor = ActorUtils.GetActor(target.Uid);
            AreaUtils.CreateArea(TempConfig.AttackAreaId, Actor.Uid, 1, Actor.Pos, Actor.Dir, actor.Uid);
        }
        
        private void StopAttack() {
            target = null;
            Actor.GetComponent<AnimCom>()?.PlayAnim("Idle");
        }
    }
}