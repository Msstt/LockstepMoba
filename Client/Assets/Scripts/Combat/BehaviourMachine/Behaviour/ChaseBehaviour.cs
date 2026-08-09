using Combat.Actor;
using Combat.Skill;

namespace Combat.BehaviourMachine {
    public class ChaseBehaviour : ActorBehaviour {
        public override SkillType SkillType => SkillType.Move;
        
        private FloatF chaseDistance;
        private FloatF patrolDistance;
        
        private Actor.Actor target;
        private bool waitStart;
        
        public ChaseBehaviour(Machine machine, FloatF patrolDistance, FloatF chaseDistance) : base(machine) {
            this.patrolDistance = patrolDistance;
            this.chaseDistance = chaseDistance;
            if (patrolDistance >= chaseDistance) {
                Log.Warning("ChaseBehaviour: patrolDistance should be less than chaseDistance.");
            }
        }
        
        public override bool Evaluate() {
            if (target != null) {
                if (Vector3F.Distance(Actor.Pos, target.Pos) > chaseDistance) {
                    return false;
                }
                
                return true;
            }
            
            foreach (var uid in NavmeshUtils.RaycastInCircle(Actor.Pos, patrolDistance)) {
                Actor.Actor actor = ActorUtils.GetActor(uid);
                if (actor != null && actor.Camp != Actor.Camp) {
                    StartChase(actor);
                    break;
                }
            }

            return target != null;
        }

        public override void Execute(int frame) {
            if (waitStart) {
                waitStart = false;
                
                // stop_distance = 0 进入攻击范围后自动被高优先级的 AttackBehaviour 打断 
                Actor.GetComponent<MoveCom>().MoveToActorByPath(target.Uid, 0, StopChase, StopChase);
                Actor.GetComponent<AnimCom>()?.PlayAnim("Move");
            }
        }

        public override void OnAbort() {
            Actor.GetComponent<MoveCom>().ForceFail();
        }

        private void StartChase(Actor.Actor target) {
            this.target = target;
            waitStart = true;
        }
        
        private void StopChase() {
            target = null;
            Actor.GetComponent<AnimCom>()?.PlayAnim("Idle");
        }
    }
}