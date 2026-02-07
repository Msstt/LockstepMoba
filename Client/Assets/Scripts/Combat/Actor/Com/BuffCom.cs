using System.Collections.Generic;
using System.Linq;
using Combat.Buff;

namespace Combat.Actor {
    public class BuffCom : Com {
        private static readonly int InValidEndFrame = -1;
        
        private class BuffInfo {
            public Buff.Buff buff;
            public int endFrame;
        }
        
        private Dictionary<int, List<BuffInfo>> buffs = new Dictionary<int, List<BuffInfo>>();
        private List<BuffInfo> toRemoveInfo = new List<BuffInfo>();
        private List<int> toRemoveBuff = new List<int>();
        
        public override void Update(int frame) {
            toRemoveBuff.Clear();
            foreach (var (buffId, list) in buffs) {
                toRemoveInfo.Clear();
                foreach (var info in list) {
                    if (info.endFrame != InValidEndFrame && info.endFrame <= frame) {
                        info.buff.Dispose();
                        toRemoveInfo.Add(info);
                    }
                }
                foreach (var info in toRemoveInfo) {
                    list.Remove(info);
                }

                if (!list.Any()) {
                    toRemoveBuff.Add(buffId);
                }
                
                foreach (var info in list) {
                    info.buff.Update();
                }
            }
            foreach (var id in toRemoveBuff) {
                buffs.Remove(id);
            }
        }

        public override void Destroy() {
            foreach (var list in buffs.Values) {
                foreach (var info in list) {
                    info.buff.Dispose();
                }
            }
        }

        public void AddBuff(int buffId, int adderId, int level) {
            BuffConfig config = Config.Buff[buffId];
            BuffInfo toMerge = GetToMerge(config, adderId);
            if (toMerge != null) {
                toMerge.buff.Merge(adderId, level);
                toMerge.endFrame = GetEndFrame(config);
            } else {
                Buff.Buff buff = new Buff.Buff(buffId, Actor.Uid, adderId, level);
                BuffInfo info = new BuffInfo {
                    buff = buff,
                    endFrame = GetEndFrame(config),
                };
                if (!buffs.ContainsKey(config.Id)) {
                    buffs[config.Id] = new List<BuffInfo>();
                }
                buffs[config.Id].Add(info);
            }
        }

        private BuffInfo GetToMerge(BuffConfig config, int adderId) {
            if (config.IsOnly) {
                return buffs.TryGetValue(config.Id, out var list) ? list.First() : null;
            } else {
                if (buffs.ContainsKey(config.Id)) {
                    foreach (var info in buffs[config.Id]) {
                        if (info.buff.AdderId == adderId) {
                            return info;
                        }
                    }
                }
                return null;
            }
        }

        private int GetEndFrame(BuffConfig config) => config.IsForever ? InValidEndFrame : TimeUtils.GetFrame(config.Time);
    }
}