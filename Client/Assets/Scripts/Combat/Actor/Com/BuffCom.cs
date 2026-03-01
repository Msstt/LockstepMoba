using Combat.Buff;
using Framework;

namespace Combat.Actor {
    public class BuffCom : Com {
        private static readonly int InValidEndFrame = -1;
        
        private class BuffInfo {
            public Buff.Buff buff;
            public int endFrame;
        }
        
        private SafeDictionary<int, SafeList<BuffInfo>> buffs = new SafeDictionary<int, SafeList<BuffInfo>>();
        
        public override void Update(int frame) {
            foreach (var (buffId, list) in buffs) {
                foreach (var info in list) {
                    if (info.endFrame != InValidEndFrame && info.endFrame <= frame) {
                        info.buff.Dispose();
                        list.Remove(info);
                    }
                }
                if (list.Count == 0) {
                    buffs.Remove(buffId);
                }
                
                foreach (var info in list) {
                    info.buff.Update();
                }
            }
        }

        public override void Destroy() {
            foreach (var (_, list) in buffs) {
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
                buff.Init();
                BuffInfo info = new BuffInfo {
                    buff = buff,
                    endFrame = GetEndFrame(config),
                };
                if (!buffs.ContainsKey(config.Id)) {
                    buffs[config.Id] = new SafeList<BuffInfo>();
                }
                buffs[config.Id].Add(info);
            }
        }

        private BuffInfo GetToMerge(BuffConfig config, int adderId) {
            if (config.IsOnly) {
                return buffs.ContainsKey(config.Id) ? buffs[config.Id].First() : null;
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