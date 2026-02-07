using Framework;
using ParadoxNotion.Design;

namespace Editor.Skill {
    [Category("移动/沿路经移动到参数位置")]
    public class MoveToPosByPath : NoParamNode {
        public override string name => "沿路经移动到参数位置";
    }
    
    [Category("移动/沿路经移动到参数目标（攻击距离内）")]
    public class MoveToActorByPathInAttackDistance : NoParamNode {
        public override string name => "沿路经移动到参数目标（攻击距离内）";
    }
    
    [Category("移动/沿路经移动到参数目标")]
    public class MoveToActorByPathInDistance : ParamNode<Combat.Skill.SkillNode.MoveToActorByPath.Param> {
        public override string name => "沿路经移动到参数目标";
    }
}