using System.Collections.Generic;
using UnityEngine;

namespace Combat.Skill {
    public class Tree {
        private static bool PrintSkillTree;
        
        private Node root;
        private readonly Dictionary<Node, List<Node>> nodes = new Dictionary<Node, List<Node>>();
        private readonly Dictionary<Node, Node> parent = new Dictionary<Node, Node>();
        
        public int Id { get; private set; }
        public SkillType Type { get; private set; }
        public bool CanAbortSelf { get; private set; }

        public Tree(int skillId) {
            SkillConfig config = Config.Skill[skillId];
            Id = config.Id;
            Type = config.SkillType;
            CanAbortSelf = config.CanAbortSelf;
            root = InitNode(config.Node);
#if UNITY_EDITOR
            PrintSkillTree = GameMgr.Instance.GMTool.PrintSkillTree;
#endif
        }

        public NodeState Execute(Context context) {
            Node curNode = context.CurNode;
            NodeState ret = NodeState.Continue;
            if (curNode == null) {
                curNode = root;
                context.ChangeNode(this, curNode);
                ret = curNode.Enter(context);
            }
            while (curNode != null) {
                if (ret == NodeState.Continue) {
                    ret = curNode.Update(context);
                }
                if (ret == NodeState.Continue) {
                    return NodeState.Continue;
                } else if (ret == NodeState.Fail) {
                    Fail(context);
                    return NodeState.Fail;
                }
                
                curNode.Exit(context);
                
                if (curNode is SelectNode select) {
                    int index = select.Select(context);
                    if (index == SelectNode.InValidIndex) {
                        Fail(context);
                        return NodeState.Fail;
                    }
                    curNode = GetNextNode(curNode, index);
                } else {
                    curNode = GetNextNode(curNode);
                }

                if (curNode != null) {
                    context.ChangeNode(this, curNode);
                    ret = curNode.Enter(context);
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
            if (PrintSkillTree) {
                Debug.Log("[SkillTree] " + GameMgr.Instance.Frame + ":   " +
                          " ActorUid: " + context.ActorUid +
                          " Finish Tree: " + context.TreeId);
            }
            Node curNode = context.CurNode;
            while (curNode != null) {
                curNode.Finish(context);
                curNode = parent.GetValueOrDefault(curNode, null);
            }
        }

        public void Fail(Context context) {
            Node curNode = context.CurNode;
            while (curNode != null) {
                curNode.Fail(context);
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