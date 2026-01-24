using Network;

namespace Combat.Skill {
    public class SkillParam {
        private Vector3F pos;
        public Vector3F Pos => PosIsValid ? pos : throw new CombatException("SkillParam Pos is not valid but accessed");
        public bool PosIsValid => pos != InvalidPos;
        
        private int uid;
        public int Uid => UidIsValid ? uid : throw new CombatException("SkillParam Uid is not valid but accessed");
        public bool UidIsValid => uid != InvalidUid;

        public SkillParam(skill_param proto) {
            pos = proto.Pos.ToVector3F();
            uid = proto.Uid;
        }

        public static skill_param CreateProto() {
            return new skill_param {
                Pos = InvalidPosProto,
                Uid = InvalidUid
            };
        }
        
        public static readonly Vector3F InvalidPos = new Vector3F(FloatF.max, FloatF.max, FloatF.max);
        private static readonly vector_f InvalidPosProto = InvalidPos.ToProto();
        public static readonly int InvalidUid = -1;
    }
}