using System;
using System.Linq;
using System.Reflection;

namespace RecipeBrowserToMagicStorageExtra.Utils
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

        public static void SetField(object obj, string name, object value, Type classType = null)
        {
            var flags = obj != null
                ? BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                : BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

            var type = obj != null ? obj?.GetType() : classType;
            var field = type?.GetField(name, flags);
            if (field != null)
	            field.SetValue(obj, value);
            else
	            RecipeBrowserToMagicStorageExtra.Instance.Logger.Error("Couldn't find field named " + name);
        }

        public static void SetProperty(object obj, string name, object value, Type classType = null)
        {
	        var flags = obj != null
		        ? BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
		        : BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

	        var type = obj != null ? obj?.GetType() : classType;
	        var prop = type?.GetProperty(name, flags);
	        if (prop != null)
		        prop.SetValue(obj, value);
	        else
		        RecipeBrowserToMagicStorageExtra.Instance.Logger.Error("Couldn't find property named " + name);
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
