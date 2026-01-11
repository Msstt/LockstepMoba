// MoveType 之间互相不能转移，需要调用方主动释放，所以实现方式类似状态机但不完全是

using System;
using System.Collections.Generic;
using Combat.Actor.Move;
using UnityEngine;

namespace Combat.Actor {
    public enum MoveType {
        None,
        PosByPath,
    }
    
    public class MoveCom : Com {
        private MoveType curType = MoveType.None;
        
        private float smoothPosSpeed = 0f;
        private readonly float smoothDirSpeed = 12f;

        private readonly Dictionary<MoveType, MoveComStatus> typeToStatus = new Dictionary<MoveType, MoveComStatus>();
        
        private Vector3F targetPos;
        public Vector3F TargetPos => targetPos;
        
        private List<Vector3F> path = new List<Vector3F>();
        public IReadOnlyList<Vector3F> Path => path;
        
        private Action finishCallback, failCallback;
        
        public float SmoothPosSpeed => smoothPosSpeed;

        public override void Awake() {
            typeToStatus[MoveType.PosByPath] = new PosByPath(this);
        }

        public override void Update(int frame) {
            if (curType != MoveType.None) {
                typeToStatus[curType].Update(frame);
            }

            FloatF dis = Vector3F.Distance(Actor.Pos, Actor.Go.transform.position.ToVector3F());
            if (dis == FloatF.zero) {
                smoothPosSpeed = 0;
            } else {
                smoothPosSpeed = (dis / GameMgr.Instance.DeltaTime).ToFloat();
            }
        }
        
        public override void RenderUpdate() {
            Actor.Go.transform.position = Vector3.MoveTowards(Actor.Go.transform.position, Actor.Pos.ToVector3(), smoothPosSpeed * Time.deltaTime);
            
            Quaternion targetRot = Quaternion.LookRotation(Actor.Dir.ToVector3());
            Actor.Go.transform.rotation = Quaternion.Slerp(Actor.Go.transform.rotation, targetRot, smoothDirSpeed * Time.deltaTime);
            
            // UpdateHeight();
        }
        
        // 表现层的 y，不需要同步，所以直接用 Unity
        private void UpdateHeight() {
            Vector3 pos = Actor.Go.transform.position;
            Ray ray = new Ray(new Vector3(pos.x, 1000, pos.z), new Vector3(0, -2000, 0));
            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, LayerMask.GetMask("Map"))) {
                Actor.Go.transform.position = new Vector3(pos.x, hitInfo.point.y, pos.z);
            } else {
                Log.Warning("Actor Raycast for height failed: " + pos);
            }
        }

        private void Clear() {
            curType = MoveType.None;
            targetPos = new Vector3F();
            path = new List<Vector3F>();
            finishCallback = failCallback = null;
        }

        private void Finish() {
            finishCallback?.Invoke();
            Clear();
        }

        private void Fail() {
            failCallback?.Invoke();
            Clear();
        }

        private void CalcPath(Vector3F pos, bool force = true) {
            NavmeshUtils.FindPath(Actor.Pos, pos, (path) => { // TODO radius
                // for (int i = 0; i < path.Count - 1; i++) {
                //     DebugUtils.DrawLine(path[i], path[i + 1]);
                // }
                path.RemoveAt(0);
                this.path = path;
            }, force);
        }
        
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
        
        #region 接口

        public void ForceFail() {
            Fail();
        }
        
        public void MoveToPosByPath(Vector3F target, Action finish = null, Action fail = null) {
            if (curType != MoveType.None) {
                return;
            }
            curType = MoveType.PosByPath;
            targetPos = target;
            finishCallback = finish;
            failCallback = fail;
            typeToStatus[curType].Enter();
        }
        
        #endregion
    }
}