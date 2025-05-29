using System.Diagnostics.CodeAnalysis;

namespace d9.utl;
/// <summary>
/// Provides a way to check whether an object which may not have been initialized correctly, e.g. a
/// <see cref="Config.TryLoad{T}(string?, bool)">config file loaded from json</see>, was in fact
/// loaded correctly.
/// </summary>
public interface IValidityCheck
{
    /// <summary>
    /// Checks the implementing object for validity, and, if it is invalid, outputs the <paramref
    /// name="reason"/> it is not valid.
    /// </summary>
    /// <param name="reason">
    /// The reason the object is invalid, if it is invalid. Should be ignored if the method returns
    /// <see langword="true"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if this object is fully and properly initialized, or <see
    /// langword="false"/> otherwise.
    /// </returns>
    public abstract bool IsValid([NotNullWhen(false)] out string? reason);
}