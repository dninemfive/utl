using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Collections;
using System.Runtime.CompilerServices;
namespace d9.utl;
public static class PrettyPrintExtensions
{

    /// <summary>
    /// Prints all fields and properties on a specified object.
    /// </summary>
    /// <param name="obj">The object to print.</param>
    /// <param name="indent">How many indents to print before each member in this specific object. Used internally.</param>
    /// <returns>A pretty-printed object.</returns>
    public static string PrettyPrint(this object? obj, string indent = "")
    {
        if (indent.Length > 12)
            return "too much recursion!";
        if (obj is null)
            return Constants.NullString;
        if (obj is Type t)
            return t.AngleBracketGenericName();
        Type objType = obj.GetType();
        if (obj is IEnumerable enumerable)
            return enumerable.EnumerableString();
        if (objType.IsPrimitive)
            return $"{obj}";
        IEnumerable<MemberInfo> members = objType.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                                 .Where(x => (x is FieldInfo fi && !fi.IsStatic) || (x is PropertyInfo pi && !(pi.GetGetMethod()?.IsStatic ?? false)));
        string result = $"{objType.AngleBracketGenericName()} {{", nextIndent = indent + "  ";
        foreach (MemberInfo member in members)
        {
            // https://stackoverflow.com/a/1593822
            if (member.GetCustomAttribute<CompilerGeneratedAttribute>() is not null)
                continue;
            if(member.Summary(obj, out object? value) is string summary)
            {
                result += $"\n{nextIndent}{summary}{value.PrettyPrint(nextIndent)}";
            }
        }
        result += $"\n{indent}}}";
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
    private static string EnumerableString(this IEnumerable enumerable)
    {
        string result = "[";
        foreach (object? item in enumerable)
        {
            if(item is IEnumerable enumerable2)
            {
                result += $"{enumerable2.EnumerableString()}, ";
            } 
            else
            {
                result += $"{item.PrintNull()}, ";
            }
        }
        if (result.Length > 2)
            result = result[..^2];
        return $"{result}]";
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
}