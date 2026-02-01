using Combat.Skill;
using Network;
using UnityEngine;

namespace InputSystem.Command {
    public class Attack : Command {
        private int? targetUid;
        
        public override void Update() {
            if (Input.GetMouseButtonDown(1)) {
                int? uid = InputUtils.GetMouseActorUid();
                if (uid.HasValue && !ActorUtils.IsSameCamp(uid.Value)) {
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