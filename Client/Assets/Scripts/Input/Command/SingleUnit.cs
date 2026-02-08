using Combat.Skill;
using Network;
using UnityEngine;

namespace InputSystem.Command {
    public class SingleUnit : Command {
        private KeyCode key;
        private int? targetUid;
        
        public SingleUnit(KeyCode key) {
            this.key = key;
        }
        
        public override void Update() {
            if (Input.GetKeyDown(key)) {
                int? uid = InputUtils.GetMouseActorUid();
                if (uid.HasValue) {
                    targetUid = uid;
                }
            }
        }

        public override skill_param GetProto() {
            if (!targetUid.HasValue) {
                return null;
            }
            var msg = SkillParam.CreateProto();
            msg.Uid = targetUid.Value;
            targetUid = null;
            return msg;
        }
    }
}