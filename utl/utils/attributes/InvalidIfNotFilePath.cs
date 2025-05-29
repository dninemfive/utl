namespace d9.utl;
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class InvalidIfNotFilePath() : Attribute, IValidityCheck
{
    public string? InvalidReason(object? value)
    {
        string? path = value as string;
        if (!File.Exists(path))
        {
            return $"`{path.PrintNull()}` is not a valid file";
        }
        return null;
    }
}