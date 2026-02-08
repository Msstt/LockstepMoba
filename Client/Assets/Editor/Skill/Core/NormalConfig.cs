using Combat.Actor;
using Combat.Skill;
using InputSystem;
using Sirenix.OdinInspector;

namespace Editor.Skill {
    public class NormalConfig {
        [ReadOnly]
        public int Id;
        
        [LabelText("名称")]
        public string Name;
        [LabelText("备注")]
        public string Note;
        
        [DrawWithUnity]
        [LabelText("技能类型")]
        public SkillType SkillType;
        
        [DrawWithUnity]
        [LabelText("输入类型")]
        public CommandType InputType;
        
        [LabelText("CD")]
        public LevelNumber<FloatF> CD;
        
        [LabelText("是否可自我打断")]
        public bool CanAbortSelf;

        public SkillConfig Export() {
            SkillConfig config = new SkillConfig();
            config.Id = Id;
            config.Name = Name;
            config.SkillType = SkillType;
            config.InputType = InputType;
            config.CD = CD;
            config.CanAbortSelf = CanAbortSelf;
            return config;
        }
    }
    
    public abstract class NoParamNode : EffectNode {
        protected override object Params => null;
    }
    
    public abstract class ParamNode<T> : EffectNode {
        [OdinTree] public T param;
        protected override object Params => param;
    }
}