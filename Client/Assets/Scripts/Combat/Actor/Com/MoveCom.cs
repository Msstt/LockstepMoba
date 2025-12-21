using System.Collections.Generic;
using UnityEngine;

namespace Combat.Actor {
    public enum MoveType {
        None,
        MoveToPos,
    }
    
    public class MoveCom : Com {
        private MoveType curType = MoveType.None;
        
        private Vector3F targetPos;
        
        private List<Vector3F> path = new List<Vector3F>();

        private float smoothSpeed = 0f;

        public override void Update() {
            if (curType == MoveType.MoveToPos) {
                MoveToPosUpdate();
            }
        }
        
        public override void RenderUpdate() {
            if (Vector3.Distance(Actor.Pos.ToVector3(), Actor.Go.transform.position) <= smoothSpeed * Time.deltaTime) {
                Actor.Go.transform.position = Actor.Pos.ToVector3();
            } else {
                Vector3 pos = smoothSpeed * Time.deltaTime * (Actor.Pos.ToVector3() - Actor.Go.transform.position).normalized;
                Actor.Go.transform.position += pos;
            }
            UpdateHeight();
        }

        public void Clear() {
            curType = MoveType.None;
            targetPos = new Vector3F();
        }

        #region MoveToPos

        public void MoveToPos(Vector3F target) {
            Clear();
            curType = MoveType.MoveToPos;
            targetPos = target;
            GetPath(targetPos, true);
        }
        
        private void MoveToPosUpdate() {
            Vector3F lastPos = Actor.Pos;
            FloatF remDis = Actor.Stats.MoveSpeed * GameMgr.Instance.DeltaTime;
            while (remDis > 0) {
                if (Vector3F.Distance(Actor.Pos, targetPos) < 10) { // TODO
                    Clear();
                    return;
                }
                if (path.Count == 0) {
                    GetPath(targetPos, true);
                }
                if (path.Count == 0) {
                    Log.Error("Actor findPath failed: " + Actor.Pos + " -> " + targetPos);
                }
                FloatF dis = Vector3F.Distance(Actor.Pos, path[0]);
                Actor.Dir = path[0] - Actor.Pos;
                if (remDis >= dis) {
                    remDis -= dis;
                    Actor.SetPos(path[0]);
                    path.RemoveAt(0);
                } else {
                    Vector3F dir = (path[0] - Actor.Pos).Normalized();
                    Actor.SetPos(Actor.Pos + dir * remDis);
                    remDis = 0;
                }
            }
            smoothSpeed = (Vector3F.Distance(Actor.Pos, lastPos) / GameMgr.Instance.DeltaTime).ToFloat();
        }

        #endregion

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

        private void GetPath(Vector3F pos, bool force) {
            NavmeshUtils.FindPath(Actor.Pos, pos, (path) => { // TODO
                path.RemoveAt(0);
                this.path = path;
            }, force);
        }
    }
}