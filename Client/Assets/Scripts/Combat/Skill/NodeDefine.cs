using Combat.Skill.SkillNode;

namespace Combat.Skill {
    public enum NodeType {
        MoveToPosByPath = 1,
        PlayAnim = 2,
        AbortSkill = 3,
    }
    
    public static class NodeFactory {
        public static Node CreateNode(NodeConfig config) {
            switch ((NodeType)config.Type) {
                case NodeType.MoveToPosByPath:
                    return new MoveToPosByPath();
                case NodeType.PlayAnim:
                    return new PlayAnim(config.Params);
                case NodeType.AbortSkill:
                    return new AbortSkill(config.Params);
                default:
                    throw new CombatException("Node type doesn't exist: " + config.Type);
            }
        }
    }
}