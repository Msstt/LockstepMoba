using Combat.Actor;
using Combat.Skill;

namespace Combat.BehaviourMachine {
    public class AttackBehaviour : ActorBehaviour {
        public override SkillType SkillType => SkillType.NormalAttack;
        
        private int attackWindupFrame;
        private int attackFrame;
        private bool waitAttack;
        private Actor.Actor target;
        private int startFrame;

        public AttackBehaviour(Machine machine, FloatF attackWindupRatio) : base(machine) {
            attackWindupFrame = TimeUtils.GetFrameCount(attackWindupRatio * Actor.Stats.AttackSpeed);
            attackFrame = TimeUtils.GetFrameCount(Actor.Stats.AttackSpeed);
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
            waitAttack = true;
            Actor.GetComponent<AnimCom>()?.PlayAnim("Attack");
        }

        private void Attack() {
            if (target != null) {
                return;
            }
            HitInfo hitInfo = new HitInfo {
                attacker = Actor.Uid,
                damage = new Damage {
                    physical = Actor.Stats.Attack,
                    magic = 0,
                    @true = 0,
                }
            };
            target.OnHit(hitInfo);
        }
        
        private void StopAttack() {
            target = null;
            Actor.GetComponent<AnimCom>()?.PlayAnim("Idle");
        }
    }
}