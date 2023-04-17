using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl
{
    public static class ReflectionUtils
    {
        public static IEnumerable<Assembly> AllLoadedAssemblies(Func<Assembly, bool>? selector = null)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) if (selector is null || selector(assembly)) yield return assembly;
        }
        public static IEnumerable<Type> AllLoadedTypes(Func<Type, bool>? selector = null)
        {
            foreach(Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes()) if (selector is null || selector(type)) yield return type;
            }
        }
        public static bool HasCustomAttribute(this Type type, Type attributeType) => type.GetCustomAttribute(attributeType) is not null;
        public static IEnumerable<Type> AllLoadedTypesWithAttribute(Type attributeType) => AllLoadedTypes(x => x.HasCustomAttribute(attributeType));
        public static bool HasCustomAttribute(this Assembly assembly, Type attributeType) => assembly.GetCustomAttribute(attributeType) is not null;
    }
}
