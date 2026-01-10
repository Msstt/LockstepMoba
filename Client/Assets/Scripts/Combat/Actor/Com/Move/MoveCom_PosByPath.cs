
namespace Combat.Actor.Move {
    public class PosByPath : MoveCom.MoveComStatus {
        private int index = 0;
        private int lastEventFrame = -1;

        public PosByPath(MoveCom com) : base(com) {
        }
        
        public override void Enter() {
            index = 0;
            CalcPath(com.TargetPos);
        }

        public override void Update(int frame) {
            FloatF remDis = Actor.Stats.MoveSpeed.Value * GameMgr.Instance.DeltaTime;
            while (remDis > 0) {
                if (Vector3F.Distance(Actor.Pos, com.TargetPos) < 1) { // TODO radius
                    Finish();
                    return;
                }
                if (index >= com.Path.Count) {
                    index = 0;
                    CalcPath(com.TargetPos);
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

        public override void Exit() {
        }
    }
}