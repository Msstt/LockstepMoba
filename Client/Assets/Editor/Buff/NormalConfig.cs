using Combat.Buff;
using ParadoxNotion;
using Sirenix.OdinInspector;

namespace Editor.Buff {
    public class NormalConfig {
        [ReadOnly]
        public int Id;
        
        [LabelText("名称")]
        public string Name;
        [LabelText("备注")]
        public string Note;
        
        [LabelText("是否是永久 BUFF")]
        public bool IsForever;
        [LabelText("持续时间")]
        public FloatF Time;
        
        [LabelText("全局唯一")]
        public bool IsOnly;
        [LabelText("最大层数")]
        public int MaxCount;

        public BuffConfig Export() {
            BuffConfig config = new BuffConfig();
            config.Id = Id;
            config.Name = Name;
            config.IsForever = IsForever;
            config.Time = Time;
            config.IsOnly = IsOnly;
            config.MaxCount = MaxCount;
            return config;
        }
    }
}