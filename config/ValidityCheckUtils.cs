using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace d9.utl;
/// <summary>
/// Useful extension methods to use for <see cref="IValidityCheck"/>s.
/// </summary>
public static class ValidityCheckUtils
{
    /// <summary>
    /// Checks whether any fields on the specified object are <see langword="null"/> and lists which ones, if any.
    /// </summary>
    /// <param name="owner">The object whose fields to check for <see langword="null"/> values.</param>
    /// <param name="nullFields">The fields, if any, which are <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if any fields on the <paramref name="owner"/> were <see langword="null"/>, or <see langword="false"/> otherwise.</returns>
    public static bool AnyFieldsAreNull(this object owner, out IEnumerable<FieldInfo> nullFields)
    {
        Type ownerType = owner.GetType();
        List<FieldInfo> result = new();
        foreach(FieldInfo field in  ownerType.GetFields())
        {
            if (field.GetValue(owner) is null)
                result.Add(field);
        }
        nullFields = result;
        return nullFields.Any();
    }
    /// <summary>
    /// Checks whether any properties on the specified object are <see langword="null"/> and lists which ones, if any.
    /// </summary>
    /// <param name="owner">The object whose properties to check for <see langword="null"/> values.</param>
    /// <param name="nullProperties">The fields, if any, which are <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if any fields on the <paramref name="owner"/> were <see langword="null"/>, or <see langword="false"/> otherwise.</returns>
    public static bool AnyPropertiesAreNull(this object owner, out IEnumerable<PropertyInfo> nullProperties)
    {
        Type ownerType = owner.GetType();
        List<PropertyInfo> result = new();
        foreach (PropertyInfo field in ownerType.GetProperties())
        {
            if (field.GetValue(owner) is null)
                result.Add(field);
        }
        nullProperties = result;
        return nullProperties.Any();
    }
    /// <param name="members">The members whose names to return.</param>
    /// <returns>The <see cref="MemberInfo.Name">names</see> of the specified <paramref name="members"/>.</returns>
    public static IEnumerable<string> Names(this IEnumerable<MemberInfo> members)
        => members.Select(x => x.Name);
    /// <summary>
    /// Checks whether a given object implementing <see cref="IValidityCheck"/> is both non-<see langword="null"/> and valid.
    /// </summary>
    /// <param name="ivc">The object to check, which may be <see langword="null"/>.</param>
    /// <param name="invalidReason">The reason, if any, that the object is invalid.</param>
    /// <returns><see langword="true"/> if <paramref name="ivc"/> is not <see langword="null"/> <b>AND</b> it <see cref="IValidityCheck.IsValid(out string?)">IsValid</see>; <see langword="false"/> otherwise.</returns>
    public static bool IsNonNullAndValid(this IValidityCheck? ivc, [NotNullWhen(false)] out string? invalidReason)
    {
        if (ivc is null)
        {
            invalidReason = $"object is null";
            return false;
        }
        return ivc.IsValid(out invalidReason);
    }
}
