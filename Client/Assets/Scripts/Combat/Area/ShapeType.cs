using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

namespace Combat.Area {
    [DrawWithUnity]
    public enum ShapeType {
        None = 0,
        Circle = 1,
        Rect = 2,
    }
    
    public static class ShapeFactory {
        public static Shape CreateShape(ShapeType type, JToken json) {
            switch (type) {
                case ShapeType.None:
                    return new None();
                case ShapeType.Circle:
                    return ParseParam<Circle>(json);
                case ShapeType.Rect:
                    return ParseParam<Rect>(json);
                default:
                    throw new CombatException("Shape type doesn't exist: " + type);
            }
        }
        
        private static T ParseParam<T>(JToken json) {
            T param = json.ToObject<T>();
            if (param == null) {
                throw new CombatException($"Shape ParseParam {typeof(T).Name} is null");
            }
            return param;
        }
    }
}
