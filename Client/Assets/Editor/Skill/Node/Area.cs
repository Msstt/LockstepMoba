using ParadoxNotion.Design;

namespace Editor.Skill {
    [Category("区域")]
    public class CreateAreaAtSelf : ParamNode<Combat.Skill.SkillNode.CreateAreaAtSelf.Param> {
        public override string name => "在自身位置创建区域";
    }
    
    [Category("区域")]
    public class CreateAreaBySingleDir : ParamNode<Combat.Skill.SkillNode.CreateAreaBySingleDir.Param> {
        public override string name => "根据单一方向创建区域";
    }
    
    [Category("区域")]
    public class CreateAreaToTargetUid : ParamNode<Combat.Skill.SkillNode.CreateAreaToTargetUid.Param> {
        public override string name => "创建区域飞向目标单位";
    }
}