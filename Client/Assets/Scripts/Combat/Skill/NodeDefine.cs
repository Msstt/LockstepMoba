using Combat.Skill.SkillNode;

namespace Combat.Skill {
    public enum NodeType {
        MoveToPosByPath = 1,
        PlayAnim = 2,
        AbortSkill = 3,
        MoveToActorByPathInAttackDistance = 4,
        MoveToActorByPathInDistance = 5,
        DamageToSingle = 6,
        WaitForTime = 7,
        WaitForAttackWindup = 8,
        WaitForAttackBackswing = 9,
        RequestSlot = 10,
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
                case NodeType.MoveToActorByPathInAttackDistance:
                    return new MoveToActorByPath();
                case NodeType.MoveToActorByPathInDistance:
                    return new MoveToActorByPath(config.Params);
                case NodeType.DamageToSingle:
                    return new DamageToSingle(config.Params);
                case NodeType.WaitForTime:
                    return new WaitForTime(config.Params);
                case NodeType.WaitForAttackWindup:
                    return new WaitForTime(true);
                case NodeType.WaitForAttackBackswing:
                    return new WaitForTime(false);
                case NodeType.RequestSlot:
                    return new RequestSlot(config.Params);
                default:
                    throw new CombatException("Node type doesn't exist: " + config.Type);
            }
        }
    }
}