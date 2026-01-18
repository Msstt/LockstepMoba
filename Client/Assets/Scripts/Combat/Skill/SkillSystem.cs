using System.Collections.Generic;
using Framework;

namespace Combat.Skill {
    public class SkillSystem : ISkillSystem {
        private static readonly string configPath = "Config/Skill/Json/";
        
        private Dictionary<int, Tree> trees = new Dictionary<int, Tree>();
        private Dictionary<int, List<Context>> contexts = new Dictionary<int, List<Context>>();
        private List<Context> toRemove = new List<Context>();
        
        public void FrameUpdate(int frame) {
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
        }

        public void Execute(int actorUid, int skillId, SkillParam param) {
            CreateTree(skillId);
            if (GetContext(actorUid, skillId) != null) {
                Log.Warning("Actor " + actorUid + " is already executing skill " + skillId);
                return;
            }

            Context context = new Context(actorUid, skillId, param);
            contexts[skillId].Add(context);
            trees[skillId].Execute(context);
        }

        public void Abort(int actorUid, int skillId) {
            CreateTree(skillId);
            Context context = GetContext(actorUid, skillId);
            if (context != null) {
                trees[skillId].Fail(context);
                contexts[skillId].Remove(context);
            }
        }

        private void CreateTree(int skillId) {
            if (trees.ContainsKey(skillId)) {
                return;
            }

            if (!JsonHelper.LoadFromFile(configPath + skillId, out SkillConfig config)) {
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
    }
}