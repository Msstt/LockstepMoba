using Newtonsoft.Json.Linq;

namespace Combat.Area {
    public enum ShapeType {
        Circle = 1,
    }
    
    public static class ShapeFactory {
        public static Shape CreateShape(ShapeType type, JToken json) {
            switch (type) {
                case ShapeType.Circle:
                    return ParseParam<Circle>(json);
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