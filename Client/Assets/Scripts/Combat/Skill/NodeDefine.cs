namespace Combat.Skill {
    public enum NodeType {
        MoveToPos = 1,
    }
    
    public static class NodeFactory {
        public static Node CreateNode(NodeConfig config) {
            switch ((NodeType)config.Type) {
                default:
                    throw new CombatException("Node type doesn't exist: " + config.Type);
            }
        }
    }
}