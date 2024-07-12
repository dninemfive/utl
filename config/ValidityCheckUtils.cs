using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace d9.utl;
public static class ValidityCheckUtils
{
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
    public static IEnumerable<string> Names(this IEnumerable<MemberInfo> members)
        => members.Select(x => x.Name);
    public static bool IsNonNullAndValid(this IValidityCheck? ivc, [NotNullWhen(false)] out string? invalidReason)
    {
        invalidReason = null;
        if (ivc is null)
        {
            invalidReason = $"object is null";
            return false;
        }
        return ivc.IsValid(out invalidReason);
    }
}
