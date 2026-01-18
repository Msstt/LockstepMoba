using Framework;

namespace Combat.Skill {
    public class Context {
        public int ActorUid { get; private set; }
        public int TreeId { get; private set; }
        public Node CurNode { get; private set; }
        public SkillParam Param { get; private set; }

        public Context(int actorUid, int treeId, SkillParam param) {
            ActorUid = actorUid;
            TreeId = treeId;
            Param = param;
        }
        
        private readonly VariantMap variants = new VariantMap();

        public void SetValue<T>(string key, T value) => variants.Set(key, value);
        public T GetValue<T>(string key) => variants.Get<T>(key);
        public T GetValueOrDefault<T>(string key, T defaultValue) => variants.GetOrDefault(key, defaultValue);

        public void ChangeNode(Tree tree, Node nextNode) {
            if (tree.Id != TreeId) {
                throw new CombatException("Context ChangeNode TreeId Mismatch");
            }
            CurNode = nextNode;
        }
    }
}