using System.Reflection;
namespace d9.utl;
/// <summary>
/// Various utilities relating to reflection.
/// </summary>
public static class ReflectionUtils
{
    /// <summary>
    /// Tells whether the specified assembly has the specified attribute.
    /// </summary>
    /// <typeparam name="T">The type to check for. Must inherit from <see cref="Attribute"/>.</typeparam>
    /// <param name="assembly">The assembly to look for the attribute on.</param>
    /// <returns><see langword="true"/> if <c>assembly</c> has an <see cref="Attribute"/> of type <c>T</c>, or <see langword="false"/> otherwise.</returns>
    public static bool HasCustomAttribute<T>(this Assembly assembly) where T : Attribute
        => assembly.GetCustomAttribute<T>() is not null;
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
    #region Type
    /// <summary>
    /// Filters the types in all loaded assemblies based on a given function.
    /// </summary>
    /// <param name="domain">The AppDomain whose assemblies to search.</param>
    /// <returns>An <see cref="IEnumerable{T}">IEnumerable</see>&lt;<see cref="Type"/>&gt; where every element <c>t</c> is present if and only if
    /// <c>selector(t)</c> returns <see langword="true"/>.</returns>
    public static IEnumerable<Type> AllLoadedTypes(this AppDomain domain)
    //    => domain.GetAssemblies().SelectMany(x => x.GetTypes());
    {
        List<Type> result = [];
        foreach(Assembly assembly in domain.GetAssemblies())
        {
            try
            {
                foreach (Type type in assembly.GetTypes())
                    result.Add(type);
            } catch {}
        }
        return result;
    }
    /// <summary>
    /// Selects the types in loaded assemblies which have the specified attribute.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of the attribute to select. Must inherit from <see cref="Attribute"/>.</typeparam>
    /// <param name="domain">The AppDomain whose assemblies to search.</param>
    /// <returns>An <see cref="IEnumerable{T}">IEnumerable</see>&lt;<see cref="Type"/>&gt; where every element has the specified attribute.</returns>
    public static IEnumerable<Type> AllLoadedTypesWithAttribute<T>(this AppDomain domain) where T : Attribute
        => domain.AllLoadedTypes().Where(x => x.HasCustomAttribute<T>());
    /// <summary>
    /// Tells whether the specified type has the specified attribute.
    /// </summary>
    /// <typeparam name="T">The type to check for. Must inherit from <see cref="Attribute"/>.</typeparam>
    /// <param name="type">The type to look for the attribute on.</param>
    /// <returns><see langword="true"/> if <c>type</c> has an <see cref="Attribute"/> of type <c>T</c>, or <see langword="false"/> otherwise.</returns>
    public static bool HasCustomAttribute<T>(this Type type) where T : Attribute
        => type.GetCustomAttribute<T>() is not null;
    #endregion Type    
}
