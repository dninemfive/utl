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
        public static IEnumerable<Type> TypesWhere(this Assembly assembly, Func<Type, bool> selector) => assembly.GetTypes().Where(x => selector(x));
        public static IEnumerable<Type> TypesInAssembliesWhere(Func<Assembly, bool> assemblySelector, Func<Type, bool>? typeSelector = null)
        {
            foreach(Assembly assembly in AllLoadedAssemblies(assemblySelector))
            {
                if(typeSelector is null)
                {
                    foreach (Type type in assembly.GetTypes()) yield return type;
                } else
                {
                    foreach (Type type in assembly.TypesWhere(x => typeSelector(x))) yield return type;
                }
            }
        }
        public static IEnumerable<Type> TypesInAssembliesWithAttribute(Type attributeType) 
            => TypesInAssembliesWhere(x => x.HasCustomAttribute(attributeType), x => x.HasCustomAttribute(attributeType));
        public static IEnumerable<(MemberInfo member, Attribute attr)> MembersWithAttribute(this Type type)
        {
            foreach()
        }
    }
}
