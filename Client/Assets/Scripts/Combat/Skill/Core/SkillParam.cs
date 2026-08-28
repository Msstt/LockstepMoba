using Network;
using Framework;

namespace Combat.Skill {
    public class SkillParam : ICheckableData {
        private Vector3F pos;
        public Vector3F Pos => PosIsValid ? pos : throw new CombatException("SkillParam Pos is not valid but accessed");
        public bool PosIsValid => pos != InvalidPos;
        
        private int uid;
        public int Uid => UidIsValid ? uid : throw new CombatException("SkillParam Uid is not valid but accessed");
        public bool UidIsValid => uid != InvalidUid;
        
        private Vector3F dir;
        public Vector3F Dir => DirIsValid ? dir : throw new CombatException("SkillParam Dir is not valid but accessed");
        public bool DirIsValid => dir != InvalidDir;

        public SkillParam(skill_param proto) {
            pos = proto.Pos.ToVector3F();
            uid = proto.Uid;
            dir = proto.Dir.ToVector3F();
        }

        public static skill_param CreateProto() {
            return new skill_param {
                Pos = InvalidPosProto,
                Uid = InvalidUid,
                Dir = InvalidDirProto,
            };
        }
        
        public static readonly Vector3F InvalidPos = new Vector3F(FloatF.max, FloatF.max, FloatF.max);
        private static readonly vector_f InvalidPosProto = InvalidPos.ToProto();
        public static readonly int InvalidUid = -1;
        public static readonly Vector3F InvalidDir = new Vector3F(FloatF.max, FloatF.max, FloatF.max);
        private static readonly vector_f InvalidDirProto = InvalidDir.ToProto();

        public int GetStatusCode() {
            int code = StatusCode.Seed;
            code = StatusCode.Combine(code, pos);
            code = StatusCode.Combine(code, uid);
            return StatusCode.Combine(code, dir);
        }
    }
}
