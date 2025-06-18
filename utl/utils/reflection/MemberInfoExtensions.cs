using System.Reflection;

namespace d9.utl.utils.reflection;
/// <summary>
/// Extensions which make getting things from members by reflection easier.
/// </summary>
public static class MemberInfoExtensions
{
    /// <summary>
    /// Prints the fully-qualified path to <paramref name="member"/> in the format
    /// <c>[namespace::][type.]&lt;name&gt;</c>.
    /// </summary>
    /// <param name="member">The member whose path to get.</param>
    /// <returns>
    /// The fully-qualified path to <paramref name="member"/>, i.e.:
    /// <br/>- the namespace containing the type, <b>if applicable</b>,
    /// <br/>- the type which declares it, <b>if applicable</b>,
    /// <br/>- the name of the type itself
    /// </returns>
    public static string FullyQualifiedPath(this MemberInfo member)
    {
        string result = member.Name;
        Type? declaringType = member.DeclaringType;
        if (declaringType is not null)
            result = $"{declaringType.Name}.{result}";
        if (declaringType?.Namespace is not null)
            result = $"{declaringType.Namespace}::{result}";
        return result;
    }

    /// <summary>
    /// Determines whether the specified <paramref name="member"/> has an attribute of the specified
    /// type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of attribute to check for.</typeparam>
    /// <param name="member">The member to check.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="member"/> has an attribute of type 
    /// <typeparamref name="T"/>, or <see langword="false"/> otherwise.
    /// </returns>
    public static bool HasCustomAttribute<T>(this MemberInfo member)
        where T : Attribute
        => member.GetCustomAttribute<T>() is not null;
}
