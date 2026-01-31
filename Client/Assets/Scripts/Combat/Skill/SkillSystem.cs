using System.Collections.Generic;
using System.Linq;
using Framework;

namespace Combat.Skill {
    public class SkillSystem : ISkillSystem {
        private static readonly int InvalidSkillId = -1;
        private static readonly string configPath = "Config/Skill/Json/";
        
        private SortedDictionary<int, Tree> trees = new SortedDictionary<int, Tree>();
        private SortedDictionary<int, List<Context>> contexts = new SortedDictionary<int, List<Context>>();
        private Queue<Context> toRemove = new Queue<Context>();
        private List<Context> toUpdate = new List<Context>();
        
        public void FrameUpdate(int frame) {
            while (toRemove.Any()) {
                var context = toRemove.Dequeue();
                contexts[context.TreeId].Remove(context);
            }
            
            toUpdate.Clear();
            foreach (var list in contexts.Values) {
                foreach (var context in list) {
                    toUpdate.Add(context);
                }
            }
            toUpdate.Sort((a, b) => a.StartFrame.CompareTo(b.StartFrame));
            foreach (var context in toUpdate) {
                NodeState ret = trees[context.TreeId].Execute(context);
                if (ret != NodeState.Continue) {
                    toRemove.Enqueue(context);
                }
            }
        }

        public void Execute(int actorUid, int skillId, SkillParam param) {
            CreateTree(skillId);
            if (GetContext(actorUid, skillId) != null) {
                if (trees[skillId].CanAbortSelf) {
                    Abort(actorUid, skillId);
                } else {
                    return;
                }
            }

            Context context = new Context(actorUid, skillId, param, GameMgr.Instance.Frame);
            contexts[skillId].Add(context);
            trees[skillId].Execute(context);
        }

        public void Abort(int actorUid, int skillId) {
            CreateTree(skillId);
            Context context = GetContext(actorUid, skillId);
            if (context != null) {
                trees[skillId].Fail(context);
                toRemove.Enqueue(context);
            }
        }

        public void Abort(int actorUid, SkillType typeList) => Abort(actorUid, typeList, InvalidSkillId);

        public void Abort(int actorUid, SkillType typeList, int excludeSkillId) {
            foreach (var tree in trees.Values) {
                if (tree.Id == excludeSkillId) {
                    continue;
                }
                if (IsContainType(typeList, tree.Type)) {
                    Abort(actorUid, tree.Id);
                }
            }
        }

        private void CreateTree(int skillId) {
            if (trees.ContainsKey(skillId)) {
                return;
            }

            if (!JsonHelper.LoadFromRes(configPath + skillId, out SkillConfig config)) {
                throw new CombatException("Skill Config not found: " + skillId);
            }
            trees[skillId] = new Tree(config);
            contexts[skillId] = new List<Context>();
        }
        
        private Context GetContext(int actorUid, int skillId) {
            foreach (var context in contexts[skillId]) {
                if (context.ActorUid == actorUid) {
                    return context;
                }
            }
            return null;
        }

        public SkillType GetSkillType(int actorUid, int skillId) {
            CreateTree(skillId);
            // 技能释放过程中，可能会改变打断优先级，所以允许实例重写 skill type
            Context context = GetContext(actorUid, skillId);
            if (context != null) {
                int @override = context.GetValueOrDefault(Field.OverrideSkillType, -1);
                if (@override == -1) {
                    return (SkillType)@override;
                }
            }
            return trees[skillId].Type;
        }

        private bool IsContainType(SkillType list, SkillType type) => (list & type) != 0;
    }
}