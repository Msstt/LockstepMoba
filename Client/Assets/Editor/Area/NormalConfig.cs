using Combat.Area;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Editor.Area {
    public class NormalConfig {
        [ReadOnly]
        public int Id;
        
        [LabelText("名称")]
        public string Name;
        [LabelText("备注")]
        public string Note;
        
        [LabelText("预制体")]
        public string Prefab;
        
        [LabelText("持续时间")]
        public FloatF Time;
        
        [DrawWithUnity]
        [LabelText("形状类型")]
        public ShapeType shape;
        
        [ShowIf("@shape == ShapeType.Circle")]
        [LabelText("形状参数")]
        [InlineProperty]
        public Circle circle;

        public AreaConfig Export() {
            AreaConfig config = new AreaConfig();
            config.Id = Id;
            config.Name = Name;
            config.Prefab = Prefab;
            config.ShapeType = shape;
            config.Time = Time;
            switch (shape) {
                case ShapeType.Circle:
                    config.ShapeParam = JToken.FromObject(circle);
                    break;
            }
            return config;
        }
    }
}