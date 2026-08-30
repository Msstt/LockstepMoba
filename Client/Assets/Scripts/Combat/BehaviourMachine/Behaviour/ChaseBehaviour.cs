using Combat.Actor;
using Combat.Skill;
using Framework;

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
                // 超过距离后失去仇恨
                if (Vector3F.DistanceXZ(Actor.Pos, target.Pos) > chaseDistance) {
                    return false;
                }
                
                return true;
            }
            
            FloatF distance = FloatF.max;
            using (PooledList<int> uids = PooledList<int>.Get()) {
                NavmeshUtils.RaycastInCircle(Actor.Pos, patrolDistance, uids);
                foreach (int uid in uids) {
                    Actor.Actor actor = ActorUtils.GetActor(uid);
                    if (actor != null && actor.Camp != Actor.Camp) {
                        FloatF distanceXZ = Vector3F.DistanceXZ(Actor.Pos, actor.Pos);
                        if (distanceXZ < distance) {
                            distance = distanceXZ;
                            StartChase(actor);
                        }
                    }
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
