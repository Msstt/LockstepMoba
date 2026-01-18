using UnityEngine;

namespace Combat.Actor {
    public class AnimCom : Com {
        public readonly static float walkAnimSpeedFactor = 0.02f;
        public readonly static float attackAnimSpeedFactor = 1f;
        
        private Animator animator;
        
        private string lastAnim = "Idle";
        private string curAnim = "Idle";
        private float smoothPosSpeed = 0f;
        private FloatF attackSpeed = FloatF.zero;
        
        public override void Awake() {
            animator = Actor.Go.transform.Find("Prefab").GetComponent<Animator>();
            if (animator == null) {
                Log.Error($"{Actor.Uid} AnimCom Awake failed: Animator not found");
            }
        }

        public override void RenderUpdate() {
            UpdateAnimSpeed();
            
            if (lastAnim != curAnim) {
                animator.SetTrigger(curAnim);
            }
            lastAnim = curAnim;
        }

        private void UpdateAnimSpeed() {
            MoveCom moveCom = Actor.GetComponent<MoveCom>();
            if (Mathf.Abs(smoothPosSpeed - moveCom.SmoothPosSpeed) >= 1e-3f) {
                smoothPosSpeed = moveCom.SmoothPosSpeed;
                animator.SetFloat("MoveSpeed", smoothPosSpeed * walkAnimSpeedFactor);
            }

            if (attackSpeed != Actor.Stats.AttackSpeed) {
                attackSpeed = Actor.Stats.AttackSpeed;
                animator.SetFloat("AttackSpeed", attackSpeed.ToFloat() * attackAnimSpeedFactor);
            }
        }

        #region 接口

        public void PlayAnim(string animName) {
            curAnim = animName;
        }

        #endregion
    }
}