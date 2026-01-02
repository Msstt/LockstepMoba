using Network;
using UnityEngine;

namespace InputSystem.Command {
    public class MoveCommand : ICommand {
        private Vector3F? targetPos;
        
        public void Update() {
            if (Input.GetMouseButtonDown(1)) {
                Vector3F? pos = InputUtils.GetMousePos();
                if (pos.HasValue) {
                    targetPos = pos;
                }
            }
        }

        public skill_param GetProto() {
            if (!targetPos.HasValue) {
                return null;
            }
            var msg = new skill_param {
                Pos = targetPos.Value.ToProto(),
            };
            targetPos = null;
            return msg;
        }
    }
}