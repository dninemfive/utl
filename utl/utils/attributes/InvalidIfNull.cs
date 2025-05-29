namespace d9.utl;
/// <summary>
/// Checks that the specified field is not <see langword="null"/>.
/// </summary>
/// <remarks>See also <see cref="ValidityCheckUtils"/>.</remarks>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class InvalidIfNullAttribute() : Attribute, IValidityCheck
{
    /// <inheritdoc cref="IValidityCheck.InvalidReason(object?)"/>
    public string? InvalidReason(object? value)
        => value is null ? "is null" : null;
}