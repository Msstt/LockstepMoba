namespace Combat.Actor {
    public partial class MoveCom : Com {
        public class ActorByPath : MoveComStatus {
            private int index = 0;
            private int lastEventFrame = -1;

            public ActorByPath(MoveCom com) : base(com) { }

            public override void Enter() {
                if (CheckIsReach()) {
                    return;
                }
                
                index = 0;
                CalcPath();
            }

            public override void Update(int frame) {
                FloatF remDis = Actor.Stats.MoveSpeed * GameMgr.Instance.DeltaTime;
                // while (remDis > 0) {
                if (CheckIsReach()) {
                    return;
                }

                if (index >= com.Path.Count) {
                    Fail();
                    return;
                }

                Vector3F? nextMove = NextExpectMove;
                if (nextMove.HasValue) {
                    nextMove = ObstacleAvoidUtils.GetNextMove(Actor, nextMove.Value);
                    Actor.SetPos(Actor.Pos + nextMove.Value);

                    if (Vector3F.DistanceXZ(Actor.Pos, com.Path[index]) <= Actor.Stats.Radius) {
                        index++;
                    }
                }
                
                if (index >= com.Path.Count) {
                    index = 0;
                    CalcPath();
                }
                // }

                // if (frame - lastEventFrame > GameMgr.Instance.FramePerSecond) {
                    // Actor.Event.OnChangePos.Send();
                // }
            }

            public override void Exit() { }

            private void CalcPath() {
                Actor actor = ActorUtils.GetActor(com.TargetUid);
                if (actor == null) {
                    Fail();
                    return;
                }
                CalcPath(actor.Pos);
            }
            
            private bool CheckIsReach() {
                Actor actor = ActorUtils.GetActor(com.TargetUid);
                if (actor == null) {
                    Fail();
                    return true;
                }
                
                if (Vector3F.Distance(Actor.Pos, actor.Pos) < com.TargetDis) {
                    Finish();
                    return true;
                }

                return false;
            }
            
            public override Vector3F NextExpectMove {
                get {
                    if (index >= com.Path.Count) {
                        return Vector3F.zero;
                    }
                    FloatF remDis = Actor.Stats.MoveSpeed * GameMgr.Instance.DeltaTime;
                    FloatF dis = Vector3F.DistanceXZ(Actor.Pos, com.Path[index]);
                    Actor.SetDir(com.Path[index] - Actor.Pos);
                    if (remDis >= dis) {
                        return com.Path[index] - Actor.Pos;
                    } else {
                        return (com.Path[index] - Actor.Pos) * (remDis / dis);
                    }
                }
            }
        }
    }
}