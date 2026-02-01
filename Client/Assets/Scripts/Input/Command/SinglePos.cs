using Combat.Skill;
using Network;
using UnityEngine;

namespace InputSystem.Command {
    public class SinglePos : Command {
        private KeyCode key;
        private Vector3F? targetPos;
        
        public SinglePos(KeyCode key) {
            this.key = key;
        }

        public override void Update() {
            if (Input.GetKeyDown(key)) {
                Vector3F? pos = InputUtils.GetMousePos();
                if (pos.HasValue) {
                    targetPos = pos;
                }
            }
        }

        public override skill_param GetProto() {
            if (!targetPos.HasValue) {
                return null;
            }

            var msg = SkillParam.CreateProto();
            msg.Pos = targetPos.Value.ToProto();
            targetPos = null;
            return msg;
        }
    }
}