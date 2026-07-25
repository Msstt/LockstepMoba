using ParadoxNotion.Design;

namespace Editor.Skill {
    [Category("移动")]
    public class MoveToPosByPath : NoParamNode {
        public override string name => "沿路经移动到参数位置";
    }
    
    [Category("移动")]
    public class MoveToActorByPathInAttackDistance : NoParamNode {
        public override string name => "沿路经移动到参数目标（攻击距离内）";
    }
    
    [Category("移动")]
    public class MoveToActorByPathInDistance : ParamNode<Combat.Skill.SkillNode.MoveToActorByPath.Param> {
        public override string name => "沿路经移动到参数目标";
    }
    
    [Category("移动")]
    public class TeleportToPos : ParamNode<Combat.Skill.SkillNode.TeleportToPos.Param> {
        public override string name => "瞬移到参数位置";
    }
}