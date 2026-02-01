using System.Collections.Generic;
using InputSystem;
using Newtonsoft.Json.Linq;

namespace Combat.Skill {
    public class NodeConfig {
        public int Type;
        public JToken Params;
        public List<NodeConfig> Child;
    }
    
    public class SkillConfig {
        public int Id;
        public string Name;
        public SkillType SkillType;
        public CommandType InputType;
        public bool CanAbortSelf;
        public NodeConfig Node;
    }
}