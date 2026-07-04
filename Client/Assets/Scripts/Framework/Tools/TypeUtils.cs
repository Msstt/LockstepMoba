using System;

public static class TypeUtils {
    public static bool IsGenericType(object obj, Type genericType) {
        if (obj == null) {
            return false;
        }

        Type type = obj.GetType();
        while (type != null && type != typeof(object)) {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType) {
                return true;
            }

            type = type.BaseType;
        }
        
        return false;
    }
}
