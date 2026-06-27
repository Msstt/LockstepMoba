using Combat.Skill;
using Network;
using UnityEngine;

namespace InputSystem.Command {
    public class SingleDir : Command {
        private readonly KeyCode key;
        private Vector3F? targetDir;
        
        public SingleDir(KeyCode key) {
            this.key = key;
        }
        
        public override void Update() {
            if (Input.GetKeyDown(key)) {
                Vector3F? dir = InputUtils.GetMouseDir();
                if (dir.HasValue) {
                    targetDir = dir;
                }
            }
        }

        public override skill_param GetProto() {
            if (!targetDir.HasValue) {
                return null;
            }
            var msg = SkillParam.CreateProto();
            msg.Dir = targetDir.Value.ToProto();
            targetDir = null;
            return msg;
        }
    }
}