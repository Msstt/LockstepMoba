using ParadoxNotion.Design;

namespace Editor.Skill {
    [Category("区域/在自身位置创建区域")]
    public class CreateAreaAtSelf : ParamNode<Combat.Skill.SkillNode.CreateAreaAtSelf.Param> {
        public override string name => "在自身位置创建区域";
    }
}