using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace d9.utl;
public static class ValidityCheckUtils
{
    private static bool IsPropertyOrField(this MemberInfo mi, object owner, out object? value)
    {
        value = null;
        if(mi is PropertyInfo pi)
        {
            value = pi.GetValue(owner);
            return true;
        } 
        else if(mi is FieldInfo fi)
        {
            value = fi.GetValue(owner);
            return true;
        }
        return false;
    }
    public static IEnumerable<string> GetInvalidMessages(this MemberInfo mi, object owner)
    {
        if(mi.IsPropertyOrField(owner, out object? value))
        {
            foreach (IValidityCheck check in mi.GetCustomAttributes()
                                               .Where(x => x is IValidityCheck)
                                               .Select(x => (IValidityCheck)x))
                if (check.InvalidReason(value) is string s)
                    yield return s;
        }
    }
    public static bool IsValid(this object owner, [NotNullWhen(false)] out IReadOnlyDictionary<string, IEnumerable<string>> messages)
    {
        messages = null;
        Dictionary<string, IEnumerable<string>> result = new();
        foreach(MemberInfo member in owner.GetType().GetMembers())
        {
            IEnumerable<string> thisMemberMessages = GetInvalidMessages(member, owner);
            if (thisMemberMessages.Any())
                result[member.Name] = thisMemberMessages;
        }
        if(result.Any())
        {
            messages = result;
            return false;
        }
        return true;
    }
    public static bool IsValid(this object owner, [NotNullWhen(false)] out string formattedMessage)
    {
        bool result = IsValid(owner, out IReadOnlyDictionary<string, IEnumerable<string>> messages);
        formattedMessage = messages.Select(x => $"{x.Key} because it {x.Value.NaturalLanguageList("and")}")
                                   .JoinWithDelimiter($";{Environment.NewLine}");
        return result;
    }
}
