using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace d9.utl;
/// <summary>
/// Extension methods for checking whether an object is valid according to attributes on their
/// properties.
/// </summary>
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
    /// <summary>
    /// Gets the messages from invalid <see cref="IValidityCheck"/>s on the specified 
    /// <paramref name="member"/> based on its value in the <paramref name="owner"/>.
    /// </summary>
    /// <param name="member">The member to check.</param>
    /// <param name="owner">The object owning the member.</param>
    /// <returns>
    /// An IEnumerable of the messages describing invalid traits of the member. If this is not empty,
    /// the member is assumed to be invalid.
    /// </returns>
    public static IEnumerable<string> GetInvalidMessages(this MemberInfo member, object owner)
    {
        if(member.IsPropertyOrField(owner, out object? value))
        {
            foreach (IValidityCheck check in member.GetCustomAttributes()
                                               .Where(x => x is IValidityCheck)
                                               .Select(x => (IValidityCheck)x))
                if (check.InvalidReason(value) is string s)
                    yield return s;
        }
    }
    /// <summary>
    /// Checks if the specified object is valid based on the annotations on its properties and
    /// fields.
    /// </summary>
    /// <param name="owner">The object whose values to check.</param>
    /// <param name="messages">The messages describing why the object is invalid, if it is invalid.</param>
    /// <returns>
    /// <see langword="true"/> if the object is valid and <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsValid(this object owner, [NotNullWhen(false)] out IReadOnlyDictionary<string, IEnumerable<string>>? messages)
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
    /// <summary><inheritdoc cref="IsValid(object, out IReadOnlyDictionary{string, IEnumerable{string}}?)" path="/summary"/></summary>
    /// <param name="owner"><inheritdoc cref="IsValid(object, out IReadOnlyDictionary{string, IEnumerable{string}}?)" path="/param[@name='owner']"/></param>
    /// <param name="formattedMessage">A formatted message describing why the object is invalid, if it is invalid.</param>
    /// <returns><inheritdoc cref="IsValid(object, out IReadOnlyDictionary{string, IEnumerable{string}}?)" path="/returns"/></returns>
    public static bool IsValid(this object owner, [NotNullWhen(false)] out string? formattedMessage)
    {
        bool result = IsValid(owner, out IReadOnlyDictionary<string, IEnumerable<string>>? messages);
        formattedMessage = messages?.Select(x => $"{x.Key} because it {x.Value.NaturalLanguageList("and")}")
                                    .JoinWithDelimiter($";{Environment.NewLine}");
        return result;
    }
}
