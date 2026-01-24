using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Combat.Skill {
    public class NodeConfig {
        public int Type;
        public JToken Params;
        public List<NodeConfig> Child;
    }
    
    public class SkillConfig {
        public int Id;
        public int SkillType;
        public bool CanAbortSelf;
        public NodeConfig Node;
    }
}