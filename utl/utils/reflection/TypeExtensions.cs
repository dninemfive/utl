using System.Reflection;

namespace d9.utl.utils.reflection;
/// <summary>
/// Extensions which make getting things from types by reflection easier.
/// </summary>
public static class TypeExtensions
{

    /// <summary>
    /// Tells whether the specified type has the specified attribute.
    /// </summary>
    /// <typeparam name="T">The type to check for. Must inherit from <see cref="Attribute"/>.</typeparam>
    /// <param name="type">The type to look for the attribute on.</param>
    /// <returns><see langword="true"/> if <c>type</c> has an <see cref="Attribute"/> of type <c>T</c>, or <see langword="false"/> otherwise.</returns>
    public static bool HasCustomAttribute<T>(this Type type) where T : Attribute
        => type.GetCustomAttribute<T>() is not null;

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
            if ((attribute = mi.GetCustomAttribute(attributeType) as T) is not null)
                yield return (mi, attribute);
        }
    }
}
