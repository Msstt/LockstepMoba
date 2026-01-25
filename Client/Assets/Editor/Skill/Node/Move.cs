using Framework;
using ParadoxNotion.Design;

namespace Editor.Skill {
    [Category("移动/沿路经移动到参数位置")]
    public class MoveToPosByPath : EffectNode {
        public override string name => "沿路经移动到参数位置";
        protected override object Params => null;
    }
    
    [Category("移动/沿路经移动到参数目标（攻击距离内）")]
    public class MoveToActorByPathInAttackDistance : EffectNode {
        public override string name => "沿路经移动到参数目标（攻击距离内）";
        protected override object Params => null;
    }
    
    [Category("移动/沿路经移动到参数目标")]
    public class MoveToActorByPathInDistance : EffectNode {
        public override string name => "沿路经移动到参数目标";
        [OdinTree] public Combat.Skill.SkillNode.MoveToActorByPath.Param param;
        protected override object Params => param;
    }
}