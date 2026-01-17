namespace Combat.Actor {
    public partial class MoveCom : Com {
        public abstract class MoveComStatus {
            protected MoveCom com;
            protected MoveComStatus(MoveCom com) {
                this.com = com;
            }
        
            protected Actor Actor => com.Actor;
        
            public virtual void Enter() { }
            public virtual void Update(int frame) { }
            public virtual void Exit() { }

            protected void Finish() => com.Finish();
            protected void Fail() => com.Fail();
            protected void CalcPath(Vector3F pos, bool force = true) => com.CalcPath(pos, force);
        }
    }
}