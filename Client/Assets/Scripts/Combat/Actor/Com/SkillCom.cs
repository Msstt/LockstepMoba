using System.Collections.Generic;
using System.Linq;
using Combat.Skill;
using Framework;
using InputSystem;

namespace Combat.Actor {
    public class SkillCom : PersistentCom {
        private static readonly int InvalidCD = -1;
        private static readonly int InvalidSkillId = 0;
        
        private ISkillSystem skillSystem;
        private IInputSystem inputSystem;
        
        private int[] skillIds = null;
        // 技能冷却结束的帧 [skillId, endFrame]
        private readonly Dictionary<int, int> cd = new Dictionary<int, int>();
        private readonly List<int> toFinish = new List<int>();
        private int[] level = null;
        
        private HashSet<int> hasExecuted = new HashSet<int>();
        
        private ControlCom controlCom;

        // TODO 分类型
        private ChampionConfig GetChampionConfig() {
            int championId = CombatUtils.GetChampionId(Uid);
            return Config.Champion[championId];
        }

        protected override void ReLife() {
            controlCom = ActorUtils.GetCom<ControlCom>(Uid);
        }

        protected override void Init() {
            skillSystem = GameMgr.Instance.GetSystem<ISkillSystem>();
            inputSystem = GameMgr.Instance.GetSystem<IInputSystem>();
            if (skillSystem == null || inputSystem == null) {
                throw new CombatException("SkillSystem is null");
            }

            // 其他玩家改变技能不会影响输入方式
            if (Uid != CombatUtils.SelfUid) {
                inputSystem = null;
            }

            skillIds = new int[SkillUtils.SkillSlotCount];
            level = new int[SkillUtils.SkillSlotCount];
            ChampionConfig config = GetChampionConfig();
            for (int i = 0; i < SkillUtils.SkillSlotCount; i++) {
                skillIds[i] = InvalidSkillId;
                level[i] = i < config.skill.Length ? config.skill[i].initLevel : 1;
            }
            InitSkillId();
        }

        public override void Update(int frame) {
            toFinish.Clear();
            foreach (var (skillId, endFrame) in cd) {
                if (endFrame <= frame) {
                    toFinish.Add(skillId);
                }
            }
            foreach (var skillId in toFinish) {
                cd.Remove(skillId);
            }
        }

        protected override void Dead() {
            // 目前 SkillCom 没有管理技能的生命周期，所以这里粗糙的处理一下，不在执行的技能树 SkillSystem 会有容错处理
            foreach (var skillId in hasExecuted) {
                skillSystem.Abort(Uid, skillId);
            }
            hasExecuted.Clear();
        }

        private void InitSkillId() {
            ChampionConfig config = GetChampionConfig();
            for (int i = (int)SkillSlot.Move; i <= (int)SkillSlot.SkillR; i++) {
                SetSkillId((SkillSlot)i, config.skill[i].skillId);
            }
            
            ICombatSystem combat = GameMgr.Instance.GetSystem<ICombatSystem>();
            if (combat != null) {
                var (skillD, skillF) = combat.GetSummonerSkill(Uid);
                SetSkillId(SkillSlot.SkillD, skillD);
                SetSkillId(SkillSlot.SkillF, skillF);
            }
        }

        private bool IsControlAbort(int skillId) {
            SkillType skillType = Config.Skill[skillId].SkillType;
            return controlCom?.IsAbort(skillType) == true;
        }

        #region 执行

        public void ExecuteSkill(SkillSlot slot, SkillParam param) {
            if (slot < 0 || (int)slot >= SkillUtils.SkillSlotCount || skillIds == null) {
                Log.Error("Invalid skill slot: " + slot);
                return;
            }
            if (InCD(slot)) {
                return;
            }
            if (level[(int)slot] <= 0) {
                return;
            }
            if (IsControlAbort(skillIds[(int)slot])) {
                return;
            }

            hasExecuted.Add(skillIds[(int)slot]);
            skillSystem.Execute(Uid, skillIds[(int)slot], level[(int)slot], param);
        }
        
        public void ExecuteSkillAsync(int skillId, int level, SkillParam param) {
            if (InCD(skillId)) {
                return;
            }
            if (IsControlAbort(skillId)) {
                return;
            }
            
            hasExecuted.Add(skillId);
            skillSystem.ExecuteAsync(Uid, skillId, level, param);
        }

        public void AbortSkill(SkillType typeList, int excludeSkillId) {
            skillSystem.AbortAsync(Uid, typeList, excludeSkillId);
        }
        
        public void AbortSkill(SkillType typeList) {
            skillSystem.Abort(Uid, typeList);
        }

        #endregion

        #region 改变技能

        private void SetSkillId(SkillSlot slot, int skillId) {
            skillIds[(int)slot] = skillId;

            if (skillId == InvalidSkillId) {
                inputSystem?.EnableCommand(slot, false);
            } else {
                inputSystem?.ChangeCommand(slot, Config.Skill[skillId].InputType);
                inputSystem?.EnableCommand(slot, InCD(skillId));
            }
        }
        
        public void ChangeSkill(SkillSlot slot, int skillId) {
            if (slot is SkillSlot.Move or SkillSlot.Attack) {
                throw new CombatException("Move and Attack commands are fixed");
            }
            SetSkillId(slot, skillId);
        }
        
        public void ChangeSkill(SkillSlot slot, int skillId, int finishFrame) {
            ChangeSkill(slot, skillId);
            cd.Add(skillId, finishFrame);
        }

        #endregion

        #region CD
        
        public void StartCD(int skillId, FloatF time) {
            cd[skillId] = TimeUtils.GetFrame(time);
            for (int i = 0; i < skillIds.Length; i++) {
                if (skillIds[i] == skillId) {
                    inputSystem?.EnableCommand((SkillSlot)i, false);
                }
            }
        }

        private bool InCD(SkillSlot slot) => cd.ContainsKey(skillIds[(int)slot]);
        private bool InCD(int skillId) => cd.ContainsKey(skillId);

        #endregion

        #region 等级

        private List<int> skillRRequestLevel = Config.Exp.skillRRequestLevel;
        public bool SkillCanLevelUp(SkillSlot slot) {
            ChampionConfig config = GetChampionConfig();
            if ((int)slot < config.skill.Length && level[(int)slot] >= config.skill[(int)slot].maxLevel) {
                return false;
            }

            int actorLevel = ActorUtils.GetCom<LevelCom>().Level;
            if (slot == SkillSlot.SkillR && config.skill[(int)slot].maxLevel == skillRRequestLevel.Count) {
                if (actorLevel < skillRRequestLevel[level[(int)slot]]) {
                    return false;
                }
            }
            
            int totalLevel = 0;
            for (int i = (int)SkillSlot.SkillQ; i <= (int)SkillSlot.SkillR; i++) {
                totalLevel += level[i];
            }

            if (totalLevel >= actorLevel) {
                return false;
            }

            return true;
        }
        
        public bool SkillCanLevelUp() {
            if (skillIds == null) {
                return false;
            }
            for (int i = 0; i < skillIds.Length; i++) {
                if (SkillCanLevelUp((SkillSlot)i)) {
                    return true;
                }
            }
            return false;
        }

        public void LevelUpSkill(SkillSlot slot) {
            if (!SkillCanLevelUp(slot)) {
            }
            level[(int)slot]++;
            EventUtils.Send(new EventType.ChampionSkillLevelUp {
                Uid = Uid,
                Slot = slot,
                Level = level[(int)slot],
            });
        }
        
        public int GetSkillLevel(SkillSlot slot) {
            return level[(int)slot];
        }

        public override int GetStatusCode() {
            int code = StatusCode.Combine(StatusCode.Seed, Uid);
            code = StatusCode.Combine(code, skillIds?.Length ?? -1);
            if (skillIds != null) {
                foreach (int skillId in skillIds) {
                    code = StatusCode.Combine(code, skillId);
                }
            }

            code = StatusCode.Combine(code, level?.Length ?? -1);
            if (level != null) {
                foreach (int skillLevel in level) {
                    code = StatusCode.Combine(code, skillLevel);
                }
            }

            code = StatusCode.Combine(code, cd.Count);
            foreach (var pair in cd.OrderBy(pair => pair.Key)) {
                code = StatusCode.Combine(code, pair.Key);
                code = StatusCode.Combine(code, pair.Value);
            }
            return code;
        }


        #endregion
    }
}
