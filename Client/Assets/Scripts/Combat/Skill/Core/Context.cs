using System.Collections.Generic;
using Framework;

namespace Combat.Skill {
    public class Context {
        public int ActorUid { get; private set; }
        public int TreeId { get; private set; }
        public Node CurNode { get; private set; }
        public int Level { get; private set; }
        public SkillParam Param { get; private set; }
        public int StartFrame { get; private set; }

        public Context(int actorUid, int treeId, int level, SkillParam param, int startFrame) {
            ActorUid = actorUid;
            TreeId = treeId;
            Level = level;
            Param = param;
            StartFrame = startFrame;
        }
        
        private readonly VariantMap variants = new VariantMap();

        public void SetValue<T>(string key, T value) => variants.Set(key, value);
        public T GetValue<T>(string key) => variants.Get<T>(key);
        public T GetValueOrDefault<T>(string key, T defaultValue) => variants.GetOrDefault(key, defaultValue);
        
        private readonly Dictionary<int, VariantMap> nodeVariants = new Dictionary<int, VariantMap>();

        public void SetValue<T>(int nodeId, string key, T value) {
            CreateVariantMap(nodeId);
            nodeVariants[nodeId].Set(key, value);
        }

        public T GetValue<T>(int nodeId, string key) {
            CreateVariantMap(nodeId);
            return nodeVariants[nodeId].Get<T>(key);
        }

        public T GetValueOrDefault<T>(int nodeId, string key, T defaultValue) {
            CreateVariantMap(nodeId);
            return nodeVariants[nodeId].GetOrDefault(key, defaultValue);
        }

        public void ChangeNode(Tree tree, Node nextNode) {
            if (tree.Id != TreeId) {
                throw new CombatException("Context ChangeNode TreeId Mismatch");
            }
            CurNode = nextNode;
        }

        private void CreateVariantMap(int nodeId) {
            if (!nodeVariants.ContainsKey(nodeId)) {
                nodeVariants[nodeId] = new VariantMap();
            }
        }
    }
}