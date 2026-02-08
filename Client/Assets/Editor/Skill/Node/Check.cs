using ParadoxNotion.Design;

namespace Editor.Skill {
    [Category("检测/是否是同一阵营")]
    public class IsSameCamp : NoParamSelectNode {
        public override string name => "是否是同一阵营";
    }
    
    [Category("检测/是否是范围内")]
    public class IsInRange : ParamSelectNode<Combat.Skill.SkillNode.IsInRange.Param> {
        public override string name => "是否是范围内";
    }
}