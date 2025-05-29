namespace d9.utl;
/// <summary>
/// Interface allowing attributes to perform a validity check on members.
/// </summary>
/// <remarks>See also <see cref="ValidityCheckUtils"/>.</remarks>
public interface IValidityCheck
{
    /// <summary>
    /// Checks whether an object is valid, returning a reason if it is not.
    /// </summary>
    /// <param name="value">The value of the object to check.</param>
    /// <returns>If the value is not valid, the reason why; otherwise, <see langword="null"/>.</returns>
    public string? InvalidReason(object? value);
}