using UnityEngine;

namespace Combat.Actor {
    public class AnimCom : Com {
        private Animator animator;
        
        private string lastAnim = "Idle";
        private string curAnim = "Idle";
        
        public override void Awake() {
            animator = Actor.Go.transform.Find("Prefab").GetComponent<Animator>();
            if (animator == null) {
                Log.Error($"{Actor.Uid} AnimCom Awake failed: Animator not found");
            }
        }

        public override void RenderUpdate() {
            if (lastAnim != curAnim) {
                animator.SetTrigger(curAnim);
            }
            lastAnim = curAnim;
        }

        #region 接口

        public void PlayAnim(string animName) {
            curAnim = animName;
        }

        #endregion
    }
}