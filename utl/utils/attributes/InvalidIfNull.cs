namespace d9.utl;
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class InvalidIfNullAttribute() : Attribute, IValidityCheck
{
    public string? InvalidReason(object? value)
        => value is null ? "is null" : null;
}