using System.Reflection;

namespace d9.utl.utils.reflection;
/// <summary>
/// Extensions which make getting things from assemblies by reflection easier.
/// </summary>
public static class AssemblyExtensions
{
    /// <summary>
    /// Tells whether the specified assembly has the specified attribute.
    /// </summary>
    /// <typeparam name="T">The type to check for. Must inherit from <see cref="Attribute"/>.</typeparam>
    /// <param name="assembly">The assembly to look for the attribute on.</param>
    /// <returns>
    /// <see langword="true"/> if <c>assembly</c> has an <see cref="Attribute"/> of type <c>T</c>,
    /// or <see langword="false"/> otherwise.
    /// </returns>
    public static bool HasCustomAttribute<T>(this Assembly assembly) where T : Attribute
        => assembly.GetCustomAttribute<T>() is not null;

    /// <summary>
    /// Filters the types in an assembly based on a given function.
    /// </summary>
    /// <param name="assembly">The assembly whose types to filter.</param>
    /// <param name="selector">
    /// The function to use to filter the assembly. If it returns <see langword="true"/> for a given
    /// type, that type is included; otherwise, it is not included.
    /// </param>
    /// <returns>
    /// An <see cref="IEnumerable{T}">IEnumerable</see>&lt;<see cref="Type"/>&gt; where every element
    /// <c>t</c> is present if and only <c>selector(t)</c> returns <see langword="true"/>.
    /// </returns>
    public static IEnumerable<Type> TypesWhere(this Assembly assembly, Func<Type, bool> selector) 
        => assembly.GetTypes().Where(x => selector(x));

    /// <summary>
    /// Gets the types with the specified attribute and their corresponding attributes.
    /// </summary>
    /// <typeparam name="T">The type of attribute to get.</typeparam>
    /// <param name="assembly">The assembly containing the types in question.</param>
    /// <returns>
    /// All types in <paramref name="assembly"/> which have an attribute of type <typeparamref name="T"/>,
    /// paired with the value of that attribute.
    /// </returns>
    public static IEnumerable<(Type type, T attr)> TypesWithAttribute<T>(this Assembly assembly)
        where T : Attribute
    {
        foreach (Type type in assembly.GetTypes())
            if (type.GetCustomAttribute<T>() is T attr)
                yield return (type, attr);
    }
}
