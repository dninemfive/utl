using System.Numerics;

namespace d9.utl;
/// <checkedSummary>; <b>throws</b> an exception if the result is not a valid color.</checkedSummary>
public readonly partial struct RgbColor
{
    private static RgbColor _applyBinaryOpChecked(Func<byte, byte, int> op, RgbColor a, RgbColor b)
        => _applyBinaryOp(op, a, b, byte.CreateChecked);
    private static RgbColor _applyUnaryOpChecked<T>(Func<byte, T> op, RgbColor a)
        where T : INumberBase<T>
        => _applyUnaryOp(op, a, byte.CreateChecked);
    /// <summary>
    /// <inheritdoc cref="operator +(RgbColor, RgbColor)" path="/summaryBase"/><inheritdoc cref="RgbColor" path="/checkedSummary"/>
    /// </summary>
    /// <param name="a"><inheritdoc cref="operator +(RgbColor, RgbColor)" path="/param[@name='a']"/></param>
    /// <param name="b"><inheritdoc cref="operator +(RgbColor, RgbColor)" path="/param[@name='b']"/></param>
    /// <returns><inheritdoc cref="operator +(RgbColor, RgbColor)" path="/returns"/></returns>
    public static RgbColor operator checked +(RgbColor a, RgbColor b)
        => _applyBinaryOpChecked((x, y) => x + y, a, b);
    /// <summary>
    /// <inheritdoc cref="operator -(RgbColor, RgbColor)" path="/summaryBase"/><inheritdoc cref="RgbColor" path="/checkedSummary"/>
    /// </summary>
    /// <param name="a"><inheritdoc cref="operator -(RgbColor, RgbColor)" path="/param[@name='a']"/></param>
    /// <param name="b"><inheritdoc cref="operator -(RgbColor, RgbColor)" path="/param[@name='b']"/></param>
    /// <returns><inheritdoc cref="operator -(RgbColor, RgbColor)" path="/returns"/></returns>
    public static RgbColor operator checked -(RgbColor a, RgbColor b)
        => _applyBinaryOpChecked((x, y) => x - y, a, b);
    /// <summary>
    /// <inheritdoc cref="operator *(RgbColor, RgbColor)" path="/summaryBase"/><inheritdoc cref="RgbColor" path="/checkedSummary"/>
    /// </summary>
    /// <param name="a"><inheritdoc cref="operator *(RgbColor, RgbColor)" path="/param[@name='a']"/></param>
    /// <param name="b"><inheritdoc cref="operator *(RgbColor, RgbColor)" path="/param[@name='b']"/></param>
    /// <returns><inheritdoc cref="operator *(RgbColor, RgbColor)" path="/returns"/></returns>
    public static RgbColor operator checked *(RgbColor a, RgbColor b)
        => _applyBinaryOpChecked((x, y) => x * y, a, b);
    /// <summary>
    /// <inheritdoc cref="operator /(RgbColor, RgbColor)" path="/summaryBase"/><inheritdoc cref="RgbColor" path="/checkedSummary"/>
    /// </summary>
    /// <param name="a"><inheritdoc cref="operator /(RgbColor, RgbColor)" path="/param[@name='a']"/></param>
    /// <param name="b"><inheritdoc cref="operator /(RgbColor, RgbColor)" path="/param[@name='b']"/></param>
    /// <returns><inheritdoc cref="operator /(RgbColor, RgbColor)" path="/returns"/></returns>
    public static RgbColor operator checked /(RgbColor a, RgbColor b)
        => _applyBinaryOpChecked((x, y) => x * y, a, b);
    /// <summary>
    /// <inheritdoc cref="operator *(RgbColor, float)" path="/summaryBase"/><inheritdoc cref="RgbColor" path="/checkedSummary"/>
    /// </summary>
    /// <param name="color"><inheritdoc cref="operator *(RgbColor, float)" path="/param[@name='color']"/></param>
    /// <param name="coefficient"><inheritdoc cref="operator *(RgbColor, float)" path="/param[@name='coefficient']"/></param>
    /// <returns><inheritdoc cref="operator *(RgbColor, float)" path="/returns"/></returns>
    public static RgbColor operator checked *(RgbColor color, float coefficient)
        => _applyUnaryOpChecked(x => x * coefficient, color);
    /// <inheritdoc cref="operator checked *(RgbColor, float)"/>
    public static RgbColor operator checked *(RgbColor color, double coefficient)
        => _applyUnaryOpChecked(x => x * coefficient, color);
    /// <inheritdoc cref="operator checked *(RgbColor, float)"/>
    public static RgbColor operator checked *(RgbColor color, decimal coefficient)
        => _applyUnaryOpChecked(x => x * coefficient, color);
}
