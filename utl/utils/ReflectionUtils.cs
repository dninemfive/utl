using System.Reflection;
namespace d9.utl;
/// <summary>
/// Various utilities relating to reflection.
/// </summary>
public static class ReflectionUtils
{
    #region Assembly
    /// <summary>
    /// Gets all assemblies in the specified <paramref name="domain"/> matching the specified <paramref name="predicate"/>.
    /// </summary>
    /// <param name="domain">The <see cref="AppDomain"/> whose assemblies to get.</param>
    /// <param name="predicate">
    /// A function which determines which assemblies to return. Only assemblies for which 
    /// <paramref name="predicate"/> is <see langword="true"/> are returned.</param>
    /// <returns>
    /// The assemblies in <paramref name="domain"/> matching the <paramref name="predicate"/>.
    /// </returns>
    public static IEnumerable<Assembly> AssembliesWhere(this AppDomain domain, Func<Assembly, bool> predicate)
    {
        foreach (Assembly assembly in domain.GetAssemblies())
            if (predicate is null || predicate(assembly))
                yield return assembly;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="domain"></param>
    /// <returns></returns>
    public static IEnumerable<(Assembly assembly, T attr)> AssembliesWithAttribute<T>(this AppDomain domain)
        where T : Attribute
    {
        foreach (Assembly assembly in domain.GetAssemblies())
            if (assembly.GetCustomAttribute<T>() is T t)
                yield return (assembly, t);
    }
    /// <summary>
    /// Tells whether the specified assembly has the specified attribute.
    /// </summary>
    /// <typeparam name="T">The type to check for. Must inherit from <see cref="Attribute"/>.</typeparam>
    /// <param name="assembly">The assembly to look for the attribute on.</param>
    /// <returns><see langword="true"/> if <c>assembly</c> has an <see cref="Attribute"/> of type <c>T</c>, or <see langword="false"/> otherwise.</returns>
    public static bool HasCustomAttribute<T>(this Assembly assembly) where T : Attribute
        => assembly.GetCustomAttribute(typeof(T)) is not null;
    #endregion Assembly
    #region MemberInfo
    /// <summary>
    /// Returns the members of the specified type with the specified attribute.
    /// </summary>
    /// <typeparam name="T">The type of the attribute to find.</typeparam>
    /// <param name="type">The type whose members to select.</param>
    /// <returns>An <see cref="IEnumerable{T}">IEnumerable</see>&lt;(<see cref="MemberInfo"/>, <see cref="Attribute"/>)&gt; where each element <c>t</c>
    /// is a tuple of a member which has the attribute <c>T</c> and the aforementioned attribute.</returns>
    public static IEnumerable<(MemberInfo member, T attr)> MembersWithAttribute<T>(this Type type) where T : Attribute
    {
        Type attributeType = typeof(T);
        foreach (MemberInfo mi in type.GetMembers())
        {
            T? attribute;
            if ((attribute = mi.GetCustomAttribute(attributeType) as T) is not null) yield return (mi, attribute);
        }
    }
    /// <summary>
    /// Prints the fully-qualified logical path to a specified member, i.e. the type which declares it, if applicable, and the namespace which declares it, if applicable.
    /// </summary>
    /// <param name="member">The member whose path to get.</param>
    /// <returns>The fully-qualified path to the specified member, as described above.</returns>
    public static string FullyQualifiedPath(this MemberInfo member)
    {
        string result = member.Name;
        Type? declaringType = member.DeclaringType;
        if (declaringType is not null) result = $"{declaringType.Name}.{result}";
        if (declaringType?.Namespace is not null) result = $"{declaringType.Namespace}::{result}";
        return result;
    }
    #endregion MemberInfo
    #region Type
    /// <summary>
    /// Filters the types in all loaded assemblies based on a given function.
    /// </summary>
    /// <param name="selector">The function to use to filter the assemblies. If it returns <see langword="true"/> for a given type, that type is included;
    /// otherwise, it is not included.</param>
    /// <returns>An <see cref="IEnumerable{T}">IEnumerable</see>&lt;<see cref="Type"/>&gt; where every element <c>t</c> is present if and only if
    /// <c>selector(t)</c> returns <see langword="true"/>.</returns>
    public static IEnumerable<Type> AllLoadedTypes(Func<Type, bool>? selector = null)
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.TypesWhere(x => selector is null || selector(x))) yield return type;
        }
    }
    /// <summary>
    /// Selects the types in loaded assemblies which have the specified attribute.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of the attribute to select. Must inherit from <see cref="Attribute"/>.</typeparam>
    /// <returns>An <see cref="IEnumerable{T}">IEnumerable</see>&lt;<see cref="Type"/>&gt; where every element has the specified attribute.</returns>
    public static IEnumerable<Type> AllLoadedTypesWithAttribute<T>() where T : Attribute
        => AllLoadedTypes(x => x.HasCustomAttribute<T>());
    /// <summary>
    /// Tells whether the specified type has the specified attribute.
    /// </summary>
    /// <typeparam name="T">The type to check for. Must inherit from <see cref="Attribute"/>.</typeparam>
    /// <param name="type">The type to look for the attribute on.</param>
    /// <returns><see langword="true"/> if <c>type</c> has an <see cref="Attribute"/> of type <c>T</c>, or <see langword="false"/> otherwise.</returns>
    public static bool HasCustomAttribute<T>(this Type type) where T : Attribute
        => type.GetCustomAttribute<T>() is not null;
    /// <summary>
    /// Iterates over all loaded assemblies, and from the ones with the specified attribute, selects the types with the same attribute.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of the attribute to select. Must inherit from <see cref="Attribute"/>.</typeparam>
    /// <returns></returns>
    public static IEnumerable<Type> TypesInAssembliesWithAttribute<T>() where T : Attribute
        => TypesInAssembliesWhere(x => x.HasCustomAttribute<T>(), x => x.HasCustomAttribute<T>());
    #endregion Type
    #region Assembly x Type
    /// <summary>
    /// Iterates over all loaded assembly, and from the ones which the <c>assemblySelector</c> returns <see langword="true"/> on,
    /// returns the types the <c>typeSelector</c>, if any, returns <see langword="true"/> on.
    /// </summary>
    /// <param name="assemblySelector">A function which decides whether to include a given assembly.</param>
    /// <param name="typeSelector">A function which decides whether to include a given type.</param>
    /// <returns>An <see cref="IEnumerable{T}">IEnumerable</see>&lt;<see cref="Type"/>&gt; where every element <c>t</c> causes 
    /// <c>typeSelector</c> to return <see langword="true"/>, and whose declaring assembly causes <c>assemblySelector</c> to return <see langword="true"/>.</returns>
    public static IEnumerable<Type> TypesInAssembliesWhere(Func<Assembly, bool> assemblySelector, Func<Type, bool>? typeSelector = null)
    {
        foreach (Assembly assembly in AllLoadedAssemblies(assemblySelector))
        {
            if (typeSelector is null)
            {
                foreach (Type type in assembly.GetTypes()) yield return type;
            }
            else
            {
                foreach (Type type in assembly.TypesWhere(x => typeSelector(x))) yield return type;
            }
        }
    }
    /// <summary>
    /// Filters the types in an assembly based on a given function.
    /// </summary>
    /// <param name="assembly">The assembly whose types to filter.</param>
    /// <param name="selector">The function to use to filter the assembly. If it returns <see langword="true"/> for a given type, that type is included;
    /// otherwise, it is not included.</param>
    /// <returns>An <see cref="IEnumerable{T}">IEnumerable</see>&lt;<see cref="Type"/>&gt; where every element <c>t</c> is present if and only if
    /// <c>selector(t)</c> returns <see langword="true"/>.</returns>
    public static IEnumerable<Type> TypesWhere(this Assembly assembly, Func<Type, bool> selector) => assembly.GetTypes().Where(x => selector(x));
    #endregion Assembly x Type        
}
