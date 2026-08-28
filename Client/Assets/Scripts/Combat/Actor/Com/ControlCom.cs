// 中转组件，根据 debuff 中断技能树、行为机

using System.Collections.Generic;
using System.Linq;
using Combat.Skill;
using Framework;

namespace Combat.Actor {
    public class ControlCom : Com {
        private readonly Dictionary<SkillType, int> abortCount = new Dictionary<SkillType, int>();

        public ReleaseToken Abort(SkillType typeList) {
            ChangeAbortCount(typeList, 1);

            SkillCom system = Actor.GetComponent<SkillCom>();
            system?.AbortSkill(typeList);

            return new ReleaseToken(() => ChangeAbortCount(typeList, -1));
        }

        public bool IsAbort(SkillType typeList) {
            int value = (int)typeList;
            while (value != 0) {
                int bit = value & -value;
                if (abortCount.ContainsKey((SkillType)bit)) {
                    return true;
                }

                value &= ~bit;
            }

            return false;
        }

        public override void Destroy() {
            abortCount.Clear();
        }

        private void ChangeAbortCount(SkillType typeList, int delta) {
            int value = (int)typeList;
            while (value != 0) {
                int bit = value & -value;
                SkillType type = (SkillType)bit;
                abortCount.TryGetValue(type, out int count);
                count += delta;

                if (count > 0) {
                    abortCount[type] = count;
                } else {
                    abortCount.Remove(type);
                }

                value &= ~bit;
            }
        }

        public override int GetStatusCode() {
            int code = StatusCode.Combine(StatusCode.Seed, abortCount.Count);
            foreach (var pair in abortCount.OrderBy(pair => (int)pair.Key)) {
                code = StatusCode.Combine(code, (int)pair.Key);
                code = StatusCode.Combine(code, pair.Value);
            }
            return code;
        }
    }
}
