namespace d9.utl;
/// <summary>
/// Checks that the specified field is a file which exists.
/// </summary>
/// <remarks>See also <see cref="ValidityCheckUtils"/>.</remarks>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class InvalidIfNotFile() : Attribute, IValidityCheck
{
    /// <inheritdoc cref="IValidityCheck.InvalidReason(object?)"/>
    public string? InvalidReason(object? value)
    {
        string? path = value as string;
        if (!File.Exists(path))
        {
            return $"`{path.PrintNull()}` is not an existing file path";
        }
        return null;
    }
}