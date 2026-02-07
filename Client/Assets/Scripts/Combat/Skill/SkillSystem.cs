using System.Collections.Generic;
using System.Linq;

namespace Combat.Skill {
    public class SkillSystem : ISkillSystem {
        private static readonly int InvalidSkillId = -1;
        
        private SortedDictionary<int, Tree> trees = new SortedDictionary<int, Tree>();
        private SortedDictionary<int, List<Context>> contexts = new SortedDictionary<int, List<Context>>();
        private List<Context> toRemove = new List<Context>();
        private Queue<(int, int, int, SkillParam)> toExecuteAsync = new Queue<(int, int, int, SkillParam)>();
        private Queue<(int, SkillType, int)> toAbortAsync = new Queue<(int, SkillType, int)>();
        
        private bool lockTree = false;
        
        public void FrameUpdate(int frame) {
            while (toExecuteAsync.Any()) {
                var (actorUid, skillId, level, param) = toExecuteAsync.Dequeue();
                Execute(actorUid, skillId, level, param);
            }
            
            while (toAbortAsync.Any()) {
                var (actorUid, typeList, excludeSkillId) = toAbortAsync.Dequeue();
                Abort(actorUid, typeList, excludeSkillId);
            }

            lockTree = true;
            foreach (var (skillId, list) in contexts) {
                toRemove.Clear();
                foreach (var context in list) {
                    NodeState ret = trees[skillId].Execute(context);
                    if (ret != NodeState.Continue) {
                        toRemove.Add(context);
                    }
                }
                foreach (var context in toRemove) {
                    list.Remove(context);
                }
            }
            lockTree = false;
        }

        public void Execute(int actorUid, int skillId, int level, SkillParam param) {
            CheckLock();
            CreateTree(skillId);
            if (GetContext(actorUid, skillId) != null) {
                if (trees[skillId].CanAbortSelf) {
                    Abort(actorUid, skillId);
                } else {
                    return;
                }
            }

            Context context = new Context(actorUid, skillId, level, param, GameMgr.Instance.Frame);
            contexts[skillId].Add(context);
            // trees[skillId].Execute(context);
        }

        public void Abort(int actorUid, int skillId) {
            CheckLock();
            CreateTree(skillId);
            Context context = GetContext(actorUid, skillId);
            if (context != null) {
                trees[skillId].Fail(context);
                contexts[skillId].Remove(context);
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
        
        public void ExecuteAsync(int actorUid, int skillId, int level, SkillParam param) {
            toExecuteAsync.Enqueue((actorUid, skillId, level, param));
        }
        
        public void AbortAsync(int actorUid, SkillType typeList, int excludeSkillId) {
            toAbortAsync.Enqueue((actorUid, typeList, excludeSkillId));
        }

        private void CreateTree(int skillId) {
            if (trees.ContainsKey(skillId)) {
                return;
            }
            
            trees[skillId] = new Tree(Config.Skill[skillId]);
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

        private void CheckLock() {
            if (lockTree) {
                throw new CombatException("SkillSystem Tree is locked");
            }
        }
    }
}