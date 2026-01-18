using Network;

namespace Combat.Skill {
    public class SkillParam {
        public Vector3F Pos { get; private set; }
        public int Uid { get; private set; }

        public SkillParam(skill_param proto) {
            Pos = proto.Pos.ToVector3F();
            Uid = proto.Uid;
        }
    }
}