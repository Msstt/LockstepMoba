using System;
using System.Runtime.CompilerServices;
using Combat.Skill;
using Newtonsoft.Json.Linq;
using ParadoxNotion;
using UnityEngine;
using Node = NodeCanvas.Framework.Node;

namespace Editor.Skill {
    public abstract class SkillNode : Node {
        public override Alignment2x2 commentsAlignment => Alignment2x2.Bottom;
        public override Alignment2x2 iconAlignment => Alignment2x2.Top;
        public override bool canSelfConnect => false;
        public override Type outConnectionType => typeof(SkillConnection);
        public override int maxInConnections => 1;

        public int Type => (int)System.Enum.Parse(typeof(NodeType), this.GetType().Name);
        protected abstract object Params { get; }

        public JToken Export() {
            try {
                if (Params == null) {
                    return null;
                }
                return JToken.FromObject(Params);
            } catch {
                SkillGraph.ExportError("导出节点参数失败: " + name);
                return null;
            }
        }
    }
    
    public abstract class EffectNode : SkillNode {
        public override int maxOutConnections => 1;
        public override bool allowAsPrime => false;
    }
    
    public abstract class SelectNode : SkillNode {
        public override int maxOutConnections => -1;
        public override bool allowAsPrime => false;
    }
    
    public class RootNode : EffectNode {
        public override int maxInConnections => 0;
        public override int maxOutConnections => 1;
        public override bool allowAsPrime => true;
        public override string name => "Root";

        public int Id;
        public SkillType SkillType;
        public bool CanAbortSelf;

        protected override object Params => throw new NotImplementedException();
    }
}