using System.Collections.Generic;

namespace Combat.Skill {
    public class Tree {
        private Node root;
        private readonly Dictionary<Node, List<Node>> nodes = new Dictionary<Node, List<Node>>();
        private readonly Dictionary<Node, Node> parent = new Dictionary<Node, Node>();
        
        public int Id { get; private set; }
        public SkillType Type { get; private set; }

        public Tree(SkillConfig config) {
            Id = config.Id;
            Type = (SkillType)config.SkillType;
            root = InitNode(config.Node);
        }

        public NodeState Execute(Context context) {
            Node curNode = context.CurNode;
            if (curNode == null) {
                curNode = root;
                context.ChangeNode(this, curNode);
                curNode.OnEnter(context);
            }
            while (curNode != null) {
                NodeState ret = curNode.OnUpdate(context);
                if (ret == NodeState.Continue) {
                    return NodeState.Continue;
                } else if (ret == NodeState.Fail) {
                    Fail(context);
                    return NodeState.Fail;
                } else if (ret == NodeState.NoKnow) {
                    Log.Error("Tree :" + context.TreeId + " Node OnUpdate returned NodeState.NoKnow");
                    Fail(context);
                    return NodeState.Fail;
                }
                
                curNode.OnExit(context);
                
                if (curNode is SelectNode select) {
                    curNode = GetNextNode(curNode, select.Select(context));
                } else {
                    curNode = GetNextNode(curNode);
                }

                if (curNode != null) {
                    context.ChangeNode(this, curNode);
                    curNode.OnEnter(context);
                }
            }

            Finish(context);
            return NodeState.Finish;
        }
        
        private Node GetNextNode(Node node, int index = 1) {
            if (!nodes.TryGetValue(node, out List<Node> nodeList) || nodeList.Count < index || index <= 0) {
                return null;
            }
            return nodeList[index - 1];
        }
        
        private void Finish(Context context) {
            Node curNode = context.CurNode;
            while (curNode != null) {
                curNode.OnFinish(context);
                curNode = parent.GetValueOrDefault(curNode, null);
            }
        }

        public void Fail(Context context) {
            Node curNode = context.CurNode;
            while (curNode != null) {
                curNode.OnFail(context);
                curNode = parent.GetValueOrDefault(curNode, null);
            }
        }

        private Node InitNode(NodeConfig config, int dep = 1) {
            if (dep > 100) {
                throw new CombatException("Skill Tree InitNode dep > 100, possible cycle in config");
            }
            Node node = NodeFactory.CreateNode(config);
            foreach (var childConfig in config.Child) {
                Node childNode = InitNode(childConfig, dep + 1);
                if (!nodes.ContainsKey(node)) {
                    nodes[node] = new List<Node>();
                }
                nodes[node].Add(childNode);
                parent[childNode] = node;
            }
            return node;
        }
    }
}