using NodeCanvas.Framework;

namespace Editor.Skill {
    public class SkillConnection : Connection {
    }
    public class SelectConnection : Connection {
        protected override string GetConnectionInfo() {
            int index = sourceNode.outConnections.IndexOf(this);
            if (sourceNode is SelectNode selectNode) {
                if (selectNode.BranchName.Length > index) {
                    return selectNode.BranchName[index];
                }
            }
            return "";
        }
    }
}