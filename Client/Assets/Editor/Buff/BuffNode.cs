using System;
using Combat.Buff;
using Newtonsoft.Json.Linq;
using ParadoxNotion;
using Node = NodeCanvas.Framework.Node;
using NodeCanvas.Framework;

namespace Editor.Buff {
    public class BuffConnection : Connection { }
    
    public abstract class BuffNode : Node {
        public override Alignment2x2 commentsAlignment => Alignment2x2.Bottom;
        public override Alignment2x2 iconAlignment => Alignment2x2.Top;
        public override bool canSelfConnect => false;
        public override Type outConnectionType => typeof(BuffConnection);
        public override int maxInConnections => 1;

        public EffectType Type => (EffectType)System.Enum.Parse(typeof(EffectType), this.GetType().Name);
        protected abstract object Params { get; }

        public JToken Export() {
            try {
                if (Params == null) {
                    return null;
                }
                return JToken.FromObject(Params);
            } catch {
                BuffGraph.ExportError("导出节点参数失败: " + name);
                return null;
            }
        }
    }
    
    public abstract class EffectNode<T> : BuffNode {
        public override int maxInConnections => 1;
        public override int maxOutConnections => 0;
        public override bool allowAsPrime => false;
        
        [OdinTree] public T param;
        protected override object Params => param;
    }
    
    public class RootNode : BuffNode {
        public override int maxInConnections => 0;
        public override int maxOutConnections => -1;
        public override bool allowAsPrime => true;
        public override string name => "Root";
        
        [OdinTree]
        public NormalConfig config = new NormalConfig();

        protected override object Params => throw new NotImplementedException();
    }
}