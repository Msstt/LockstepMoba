using System;
using Combat.Area;
using Newtonsoft.Json.Linq;
using ParadoxNotion;
using NodeCanvas.Framework;

namespace Editor.Area {
    public class AreaConnection : Connection { }
    
    public abstract class AreaNode : NodeCanvas.Framework.Node {
        public override Alignment2x2 commentsAlignment => Alignment2x2.Bottom;
        public override Alignment2x2 iconAlignment => Alignment2x2.Top;
        public override bool canSelfConnect => false;
        public override Type outConnectionType => typeof(AreaConnection);
        public override int maxInConnections => 1;

        public EffectType EffectType => (EffectType)System.Enum.Parse(typeof(EffectType), this.GetType().Name);
        public RaycastType RaycastType => (RaycastType)System.Enum.Parse(typeof(RaycastType), this.GetType().Name);
        protected abstract object Params { get; }

        public JToken Export() {
            try {
                if (Params == null) {
                    return null;
                }
                return JToken.FromObject(Params);
            } catch {
                AreaGraph.ExportError("导出节点参数失败: " + name);
                return null;
            }
        }
    }
    
    public abstract class EffectNode<T> : AreaNode {
        public override int maxInConnections => 1;
        public override int maxOutConnections => 0;
        public override bool allowAsPrime => false;
        
        [OdinTree] public T param;
        protected override object Params => param;
    }
    
    public abstract class RaycastNode<T> : AreaNode {
        public override int maxInConnections => 1;
        public override int maxOutConnections => -1;
        public override bool allowAsPrime => false;
        
        [OdinTree] public T param;
        protected override object Params => param;
    }
    
    public class RootNode : AreaNode {
        public override int maxInConnections => 0;
        public override int maxOutConnections => -1;
        public override bool allowAsPrime => true;
        public override string name => "Root";
        
        [OdinTree]
        public NormalConfig config = new NormalConfig();

        protected override object Params => throw new NotImplementedException();
    }
}