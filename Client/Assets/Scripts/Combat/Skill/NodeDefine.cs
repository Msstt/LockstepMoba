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
        RepeatSkill = 11,
        SetCD = 12,
        TeleportToPos = 13,
        None = 14,
        AddBuffToActor = 15,
        IsSameCamp = 16,
        IsInRange = 17,
    }
    
    public static class NodeFactory {
        public static Node CreateNode(NodeConfig config) {
            switch (config.Type) {
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
                case NodeType.RepeatSkill:
                    return new RepeatSkill();
                case NodeType.SetCD:
                    return new SetCD();
                case NodeType.TeleportToPos:
                    return new TeleportToPos(config.Params);
                case NodeType.None:
                    return new None();
                case NodeType.AddBuffToActor:
                    return new AddBuffToActor(config.Params);
                case NodeType.IsSameCamp:
                    return new IsSameCamp();
                case NodeType.IsInRange:
                    return new IsInRange(config.Params);
                default:
                    throw new CombatException("Node type doesn't exist: " + config.Type);
            }
        }
    }
}