using System;
using System.Linq;
using System.Reflection;

namespace RecipeBrowserToMagicStorage.Utils
{
    public static class ReflectionUtils
    {
        public static TField GetField<TField>(object obj, string name, Type classType = null)
        {
            var flags = obj != null
                ? BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                : BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            var type = obj != null ? obj?.GetType() : classType;

            var objectField = type?.GetField(name, flags);
            var objectValue = objectField?.GetValue(obj);

            if (objectValue != null && objectValue is TField value)
                return value;

            return default;
        }

        public static void SetValue(object obj, string name, object value, Type classType = null)
        {
            var flags = obj != null
                ? BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                : BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

            var type = obj != null ? obj?.GetType() : classType;

            var tryField = type?.GetField(name, flags);
            if (tryField != null)
            {
                tryField?.SetValue(obj, value);
                return;
            }

            var tryProperty = type?.GetProperty(name, flags);
            if (tryProperty != null)
            {
                tryProperty?.SetValue(obj, value);
                return;
            }
        }

        public static Type FindType(Assembly assembly, string typeName)
        {
            return assembly?.GetTypes().FirstOrDefault(type => type.Name == typeName);
        }

        public static MethodInfo GetMethodInfo(Type type, string typeName, BindingFlags flags = BindingFlags.Instance)
        {
            return type?.GetMethod(typeName, flags | BindingFlags.Public);
        }
    }
}
