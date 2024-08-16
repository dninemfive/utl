using System.Numerics;

namespace d9.utl;
public readonly partial struct RgbColor
{
    /// <summary>
    /// Implicitly converts the specified <paramref name="color"/> into a string.
    /// </summary>
    /// <param name="color">The color to convert.</param>
    public static implicit operator string(RgbColor color)
        => color.ToString();
    /// <summary>
    /// Implicitly converts the specified <paramref name="tuple"/> into an RgbColor.
    /// </summary>
    /// <param name="tuple">The tuple to convert.</param>
    public static implicit operator RgbColor((byte r, byte g, byte b) tuple)
        => new(tuple.r, tuple.g, tuple.b);
    /// <summary>
    /// Implicitly converts the specified <paramref name="hex"/> integer into an RgbColor.
    /// </summary>
    /// <param name="hex"><inheritdoc cref="RgbColor(int)" path="/param[@name='hex']"/></param>
    public static implicit operator RgbColor(int hex)
        => new(hex);
    /// <summary>
    /// Implicitly converts the specified <paramref name="hexCode"/> string into an RgbColor.
    /// </summary>
    /// <param name="hexCode"><inheritdoc cref="RgbColor(string)" path="/param[@name='s']"/></param>
    /// <remarks><inheritdoc cref="RgbColor(string)" path="/remarks"/></remarks>
    public static implicit operator RgbColor(string hexCode)
        => new(hexCode);
    public static implicit operator RgbColor((float r, float g, float b) tuple)
        => FromFloatingPoints(tuple.r, tuple.g, tuple.b);
    public static implicit operator RgbColor((double r, double g, double b) tuple)
        => FromFloatingPoints(tuple.r, tuple.g, tuple.b);
    public static implicit operator RgbColor((decimal r, decimal g, decimal b) tuple)
        => FromFloatingPoints(tuple.r, tuple.g, tuple.b);
}
