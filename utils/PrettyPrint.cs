using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Collections;
using System.Runtime.CompilerServices;
namespace d9.utl;
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class IncludeInPrettyPrintAttribute : Attribute { }
public static class PrettyPrintExtensions
{

    /// <summary>
    /// Prints all fields and properties on a specified object.
    /// </summary>
    /// <param name="obj">The object to print.</param>
    /// <param name="indent">How many indents to print before each member in this specific object. Used internally.</param>
    /// <returns>A pretty-printed object.</returns>
    public static string PrettyPrint(this object? obj, string indent = "", bool includeInitialType = true)
    {
        if (indent.Length > 12)
            return $"{obj?.GetType()?.AngleBracketGenericName().PrintNull()}.";
        if (obj is null)
            return Constants.NullString;
        if (obj is char c)
            return $"'{c}'";
        if (obj is string s)
            return $"\"{s}\"";
        if (obj is Type t)
            return t.AngleBracketGenericName();
        Type objType = obj.GetType();
        if (objType.IsPrimitive)
            return $"{obj}";
        string result = includeInitialType ? $"{objType.AngleBracketGenericName()} " : "", nextIndent = indent + "  ", brackets = "{}";
        if(obj is IEnumerable enumerable)
        {
            brackets = "[]";
            result += brackets.First();
            foreach(object? item in enumerable)
                result += $"\n{nextIndent}{item?.GetType().AngleBracketGenericName()} {item.PrettyPrint(nextIndent, false)},";
        }
        else
        {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            result += brackets.First();
            IEnumerable<MemberInfo> members = objType.GetMembers(bindingFlags)
                                                     .Where(IncludeInPrettyPrint);
            foreach (MemberInfo member in members)
            {
                // https://stackoverflow.com/a/1593822
                if (member.GetCustomAttribute<CompilerGeneratedAttribute>() is not null)
                    continue;
                if (member.Summary(obj, out object? value) is string summary)
                    result += $"\n{nextIndent}{summary}{value.PrettyPrint(nextIndent, false)},";
            }
        }
        result = result[..^1];
        result += $"\n{indent}{brackets.Second()}";
        return result;
    }
    private static string? Summary(this MemberInfo member, object owner, out object? value)
    {
        if(member.IsFieldOrProperty(owner, out Type? type, out value))
        {
            return $"{type.AngleBracketGenericName()} {member.Name}: ";
        }
        return null;
    }    
    private static bool IsFieldOrProperty(this MemberInfo member, object owner, [NotNullWhen(true)] out Type? type, out object? value)
    {
        type = null;
        value = null;
        if(member is FieldInfo field)
        {
            type = field.FieldType;
            value = field.GetValue(owner);
        } 
        else if(member is PropertyInfo property && !property.GetIndexParameters().Any())
        {
            type = property.PropertyType;
            value = property.GetValue(owner);
        }
        return type is not null;
    }
    public static string AngleBracketGenericName(this Type type)
    {
        string result = type.Name;
        if(type.GenericTypeArguments.Length > 0)
        {
            result = result.Split("`")[0];
            result += $"<{type.GenericTypeArguments.Select(x => x.AngleBracketGenericName()).ListNotation(brackets: null)}>";
        }
        return result;
    }
    private static bool IncludeInPrettyPrint(this MemberInfo member)
        => member.IsPublicInstanceField() || member.IsPublicInstanceProperty();
    private static bool IsPublicInstanceField(this MemberInfo member)
        => member is FieldInfo field 
        && !field.IsStatic
        && (field.IsPublic
            || field.GetCustomAttribute<IncludeInPrettyPrintAttribute>() is not null);
    private static bool IsPublicInstanceProperty(this MemberInfo member)
        => member is PropertyInfo property 
        && property.GetGetMethod() is MethodInfo getter 
        && !getter.IsStatic 
        && (getter.IsPublic 
            || getter.GetCustomAttribute<IncludeInPrettyPrintAttribute>() is not null);
}