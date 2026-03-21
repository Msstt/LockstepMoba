using Combat.Skill;
using Network;
using UnityEngine;

namespace InputSystem.Command {
    public class None : Command {
        private readonly KeyCode key;
        private bool isPressed;
        
        public None(KeyCode key) {
            this.key = key;
        }
        
        public override void Update() {
            if (Input.GetKeyDown(key)) {
                isPressed = true;
            }
        }
        public override skill_param GetProto() {
            if (!isPressed) {
                return null;
            }
            isPressed = false;
            return SkillParam.CreateProto();
        }
    }
}