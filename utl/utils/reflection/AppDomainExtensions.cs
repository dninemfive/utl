using System.Reflection;

namespace d9.utl.utils.reflection;
/// <summary>
/// Extensions for the <see cref="AppDomain"/> type which make reflection, mostly getting assemblies
/// from them, easier.
/// </summary>
public static class AppDomainExtensions
{
    /// <summary>
    /// Gets all assemblies in the specified <paramref name="domain"/> matching the specified 
    /// <paramref name="predicate"/>.
    /// </summary>
    /// <param name="domain">The <see cref="AppDomain"/> whose assemblies to get.</param>
    /// <param name="predicate">
    /// A function which determines which assemblies to return. Only assemblies for which 
    /// <paramref name="predicate"/> is <see langword="true"/> are returned.
    /// </param>
    /// <returns>
    /// The assemblies in <paramref name="domain"/> matching the <paramref name="predicate"/>.
    /// </returns>
    public static IEnumerable<Assembly> AssembliesWhere(this AppDomain domain, Func<Assembly, bool> predicate)
    {
        foreach (Assembly assembly in domain.GetAssemblies())
            if (predicate(assembly))
                yield return assembly;
    }

    /// <summary>
    /// Gets the assemblies which have an attribute of the specified type, <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the attribute to get.</typeparam>
    /// <param name="domain">The <see cref="AppDomain"/> containing the assemblies to get.</param>
    /// <returns>
    /// The assemblies with an attribute of type <typeparamref name="T"/> and paired with that attribute.
    /// </returns>
    public static IEnumerable<(Assembly assembly, T attr)> AssembliesWithAttribute<T>(this AppDomain domain)
        where T : Attribute
    {
        foreach (Assembly assembly in domain.GetAssemblies())
            if (assembly.GetCustomAttribute<T>() is T t)
                yield return (assembly, t);
    }
}