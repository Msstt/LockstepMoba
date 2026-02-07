using System.Collections.Generic;
using Combat.Skill;
using InputSystem;

namespace Combat.Actor {
    public class SkillCom : Com {
        private static readonly int InvalidCD = -1;
        
        private ISkillSystem skillSystem;
        private IInputSystem inputSystem;
        
        private int[] skillIds = null;
        // 技能冷却结束的帧
        private readonly Dictionary<int, int> cd = new Dictionary<int, int>();
        private readonly List<int> toFinish = new List<int>();
        private int[] level = null;
        
        private HashSet<int> hasExecuted = new HashSet<int>();
        
        public override void Awake() {
            skillSystem = GameMgr.Instance.GetSystem<ISkillSystem>();
            inputSystem = GameMgr.Instance.GetSystem<IInputSystem>();
            if (skillSystem == null || inputSystem == null) {
                throw new CombatException("SkillSystem is null");
            }

            // 其他玩家改变技能不会影响输入方式
            if (Actor.Uid != CombatUtils.SelfUid) {
                inputSystem = null;
            }

            skillIds = new int[SkillUtils.SkillSlotCount];
            level = new int[SkillUtils.SkillSlotCount];
            for (int i = 0; i < SkillUtils.SkillSlotCount; i++) {
                level[i] = 1;
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

        public override void Destroy() {
            // 目前 SkillCom 没有管理技能的生命周期，所以这里粗糙的处理一下，不在执行的技能树 SkillSystem 会有容错处理
            foreach (var skillId in hasExecuted) {
                skillSystem.Abort(Actor.Uid, skillId);
            }
        }

        private void InitSkillId() {
            ChampionConfig config = Config.Champion[Actor.Id];
            for (int i = (int)SkillSlot.Move; i <= (int)SkillSlot.SkillR; i++) {
                SetSkillId((SkillSlot)i, config.skillIds[i]);
            }
            
            ICombatSystem combat = GameMgr.Instance.GetSystem<ICombatSystem>();
            if (combat != null) {
                var (skillD, skillF) = combat.GetSummonerSkill(Actor.Uid);
                SetSkillId(SkillSlot.SkillD, skillD);
                SetSkillId(SkillSlot.SkillF, skillF);
            }
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

            hasExecuted.Add(skillIds[(int)slot]);
            skillSystem.Execute(Actor.Uid, skillIds[(int)slot], level[(int)slot], param);
        }
        
        public void ExecuteSkillAsync(int skillId, int level, SkillParam param) {
            if (InCD(skillId)) {
                return;
            }
            
            hasExecuted.Add(skillId);
            skillSystem.ExecuteAsync(Actor.Uid, skillId, level, param);
        }

        public void AbortSkill(SkillType typeList, int excludeSkillId) {
            skillSystem.AbortAsync(Actor.Uid, typeList, excludeSkillId);
        }

        #endregion

        #region 改变技能

        private void SetSkillId(SkillSlot slot, int skillId) {
            skillIds[(int)slot] = skillId;
            
            inputSystem?.ChangeCommand(slot, Config.Skill[skillId].InputType);
            inputSystem?.EnableCommand(slot, InCD(skillId));
        }
        
        public void ChangeSkill(SkillSlot slot, int skillId) {
            if (slot is SkillSlot.Move or SkillSlot.Attack) {
                throw new CombatException("Move and Attack commands are fixed");
            }
            SetSkillId(slot, skillId);
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
        
        

        #endregion
    }
}