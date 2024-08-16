using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.RegularExpressions;

namespace d9.utl;
/// <summary>
/// Represents a color in RGB format.
/// </summary>
/// <remarks>
/// Implemented because <see cref="System.Drawing"/> is not platform-agnostic, and therefore its <see cref="System.Drawing.Color">Color</see> class might cause issues on some platforms.
/// </remarks>
public readonly partial struct RgbColor
    : IParsable<RgbColor>
{
    /// <summary>
    /// The red component of this color.
    /// </summary>
    public readonly byte R;
    /// <summary>
    /// The green component of this color.
    /// </summary>
    public readonly byte G;
    /// <summary>
    /// The blue component of this color.
    /// </summary>
    public readonly byte B;
    /// <summary>
    /// Constructs an RGB color with the specified <paramref name="r"/>, <paramref name="g"/>, and <paramref name="b"/> components.
    /// </summary>
    /// <param name="r"><inheritdoc cref="R" path="/summary"/></param>
    /// <param name="g"><inheritdoc cref="G" path="/summary"/></param>
    /// <param name="b"><inheritdoc cref="B" path="/summary"/></param>
    public RgbColor(byte r, byte g, byte b)
    {
        R = r;
        G = g;
        B = b;
    }
    /// <summary>
    /// Constructs an RGB color from the specified integer, treated as a hexadecimal number.
    /// </summary>
    /// <param name="hex">An integer corresponding to a valid RGB color code.</param>
    /// <remarks><b>Throws</b> an <see cref="ArgumentOutOfRangeException"/> if <paramref name="hex"/> is less than 0 or greater than 0xFFFFFF (16777215 in decimal).</remarks>
    public RgbColor(int hex)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(hex, 0);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(hex, 0xFFFFFF);
        R = (byte)(hex & 0xFF0000);
        G = (byte)(hex & 0x00FF00);
        B = (byte)(hex & 0x0000FF);
    }
    private static Exception _invalidFormatException(string hexCode) 
        => new($"Cannot parse hex color from string {hexCode}: unrecognized format!");
    private static int? _fromHexString(string hexCode)
    {
        if (!_hexRegex.IsMatch(hexCode))
            return null;
        if (hexCode.StartsWith('#'))
            hexCode = hexCode[1..];
        return int.Parse(hexCode, System.Globalization.NumberStyles.HexNumber);
    }
    /// <summary>
    /// Constructs an RGB color from the specified string.
    /// </summary>
    /// <param name="hexCode">A string in the format <c>#XXXXXX</c>, where each X is a valid hexadecimal digit, and the # symbol is optional.</param>
    /// <remarks><b>Throws</b> an <see cref="Exception"/> if the string is not in the correct format.</remarks>
    public RgbColor(string hexCode)
        : this(_fromHexString(hexCode) ?? throw _invalidFormatException(hexCode)) { }
    private static IEnumerable<string> _argsOutOfRange<T>(params (T, string)[] args)
        where T : IFloatingPoint<T>
    {
        foreach((T t, string s) in args)
            if (t < T.Zero || t > T.One)
                yield return $"{s} ({t})";
    }
    public static RgbColor FromFloatingPoints<T>(T r, T g, T b)
        where T : IFloatingPoint<T>
    {
        IEnumerable<string> argsOutOfrange = _argsOutOfRange((r, nameof(r)), (g, nameof(g)), (b, nameof(b)));
        if (argsOutOfrange.Any())
            throw new Exception($"The following arguments were out of the range [0..1]: {argsOutOfrange.ListNotation(brackets: null)}");
        T maxByte = T.CreateSaturating(byte.MaxValue);
        return new(byte.CreateSaturating(r * maxByte), byte.CreateSaturating(g * maxByte), byte.CreateSaturating(b * maxByte));
    }
    /// <summary>
    /// Parses the specified string into an RGB color.
    /// </summary>
    /// <param name="hexCode"><inheritdoc cref="RgbColor(string)" path="/param[@name='hexCode']"/></param>
    /// <param name="_">Unused.</param>
    /// <returns>An RgbColor corresponding to the string specified.</returns>
    /// <remarks><inheritdoc cref="RgbColor(string)" path="/remarks"/></remarks>
    public static RgbColor Parse(string hexCode, IFormatProvider? _)
    {
        if (TryParse(hexCode, _, out RgbColor result))
            return result;
        throw _invalidFormatException(hexCode);
    }
    /// <summary>
    /// Tries to parse the spcified string into an RGB color.
    /// </summary>
    /// <param name="hexCode"><inheritdoc cref="RgbColor(string)" path="/param[@name='hexCode']"/></param>
    /// <param name="_">Unused.</param>
    /// <param name="result">The resulting <see cref="RgbColor"/>, if any.</param>
    /// <returns><see langword="true"/> if <paramref name="hexCode"/> was successfully parsed into a valid <paramref name="result"/>, or <see langword="false"/> otherwise.</returns>
    public static bool TryParse([NotNullWhen(true)] string? hexCode, IFormatProvider? _, [MaybeNullWhen(false)] out RgbColor result)
    {
        result = default;
        if (hexCode is null)
            return false;
        if(_fromHexString(hexCode) is int i)
        {
            result = i;
            return true;
        }
        return false;
    }
    private static Regex _hexRegex = GenerateHexRegex();
    /// <summary>
    /// Deconstructs the RGB color into its components.
    /// </summary>
    /// <param name="r"><inheritdoc cref="R" path="/summary"/></param>
    /// <param name="g"><inheritdoc cref="G" path="/summary"/></param>
    /// <param name="b"><inheritdoc cref="B" path="/summary"/></param>
    public void Deconstruct(out byte r, out byte g, out byte b)
    {
        r = R;
        g = G;
        b = B;
    }
    /// <summary>
    /// Represents this RGB color in the standard hexadecimal format.
    /// </summary>
    public override string ToString()
        => $"#{R.ToHex()}{G.ToHex()}{B.ToHex()}";
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
    [GeneratedRegex(@"#?[\da-fA-F]{6}")]
    private static partial Regex GenerateHexRegex();
}