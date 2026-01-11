using UnityEngine;

namespace Combat.Actor {
    public class AnimCom : Com {
        private static float walkAnimSpeedFactor = 0.02f;
        
        private Animator animator;
        
        private string lastAnim = "Idle";
        private string curAnim = "Idle";
        private float smoothPosSpeed = 0f;
        
        public override void Awake() {
            animator = Actor.Go.transform.Find("Prefab").GetComponent<Animator>();
            if (animator == null) {
                Log.Error($"{Actor.Uid} AnimCom Awake failed: Animator not found");
            }
        }

        public override void RenderUpdate() {
            UpdateWalkAnimSpeed();
            
            if (lastAnim != curAnim) {
                animator.SetTrigger(curAnim);
            }
            lastAnim = curAnim;
        }

        private void UpdateWalkAnimSpeed() {
            MoveCom moveCom = Actor.GetComponent<MoveCom>();
            if (smoothPosSpeed != moveCom.SmoothPosSpeed) {
                smoothPosSpeed = moveCom.SmoothPosSpeed;
                animator.SetFloat("MoveSpeed", smoothPosSpeed * walkAnimSpeedFactor);
            }
        }

        #region 接口

        public void PlayAnim(string animName) {
            curAnim = animName;
        }

        #endregion
    }
}