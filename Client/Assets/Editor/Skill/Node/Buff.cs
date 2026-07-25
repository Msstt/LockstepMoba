using ParadoxNotion.Design;

namespace Editor.Skill {
    public class Buff {
        [Category("Buff")]
        public class AddBuffToActor : ParamNode<Combat.Skill.SkillNode.AddBuffToActor.Param> {
            public override string name => "对参数目标施加 Buff";
        }
    }
}