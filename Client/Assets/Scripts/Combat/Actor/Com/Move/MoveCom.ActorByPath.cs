namespace Combat.Actor {
    public partial class MoveCom : Com {
        public class ActorByPath : MoveComStatus {
            private int index = 0;
            private int lastEventFrame = -1;

            public ActorByPath(MoveCom com) : base(com) { }

            public override void Enter() {
                index = 0;
                CalcPath();
            }

            public override void Update(int frame) {
                FloatF remDis = Actor.Stats.MoveSpeed * GameMgr.Instance.DeltaTime;
                while (remDis > 0) {
                    CheckIsReach();

                    if (index >= com.Path.Count) {
                        index = 0;
                        CalcPath();
                    }

                    if (index >= com.Path.Count) {
                        Fail();
                        return;
                    }

                    FloatF dis = Vector3F.Distance(Actor.Pos, com.Path[index]);
                    Actor.SetDir(com.Path[index] - Actor.Pos);
                    if (remDis >= dis) {
                        Actor.SetPos(com.Path[index]);
                        index++;
                        remDis -= dis;
                    } else {
                        Actor.SetPos(Actor.Pos + (com.Path[index] - Actor.Pos) * (remDis / dis));
                        remDis = 0;
                    }
                }

                if (frame - lastEventFrame > GameMgr.Instance.FramePerSecond) {
                    Actor.Event.OnChangePos.Send();
                }
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
            
            private void CheckIsReach() {
                Actor actor = ActorUtils.GetActor(com.TargetUid);
                if (actor == null) {
                    Fail();
                    return;
                }
                
                if (Vector3F.Distance(Actor.Pos, actor.Pos) < com.TargetDis) {
                    Finish();
                }
            }
        }
    }
}