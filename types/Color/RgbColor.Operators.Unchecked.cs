using System.Numerics;

namespace d9.utl;
/// <uncheckedSummary>, clamping the result to a valid range.</uncheckedSummary>
public readonly partial struct RgbColor
{
    private const byte _uncheckedDesc = 0;
    private static RgbColor _applyBinaryOpUnchecked(Func<byte, byte, int> op, RgbColor a, RgbColor b)
        => _applyBinaryOp(op, a, b, byte.CreateSaturating);
    private static RgbColor _applyUnaryOpUnchecked<T>(Func<byte, T> op, RgbColor color)
        where T : INumberBase<T>
        => _applyUnaryOp(op, color, byte.CreateSaturating);
    /// <summaryBase>Adds <paramref name="a"/> to <paramref name="b"/> componentwise</summaryBase>
    /// <summary>
    /// <inheritdoc cref="operator +(RgbColor, RgbColor)" path="/summaryBase"/><inheritdoc
    /// cref="RgbColor" path="/uncheckedSummary"/>
    /// </summary>
    /// <param name="a">The first color to add.</param>
    /// <param name="b">The second color to add.</param>
    /// <returns>The sum of <paramref name="a"/> and <paramref name="b"/>.</returns>
    public static RgbColor operator +(RgbColor a, RgbColor b)
        => _applyBinaryOpUnchecked((x, y) => x + y, a, b);
    /// <summaryBase>Subtracts <paramref name="b"/> from <paramref name="a"/> componentwise</summaryBase>
    /// <summary>
    /// <inheritdoc cref="operator -(RgbColor, RgbColor)" path="/summaryBase"/><inheritdoc
    /// cref="RgbColor" path="/uncheckedSummary"/>
    /// </summary>
    /// <param name="a">The color to subtract from.</param>
    /// <param name="b">The color to subtract.</param>
    /// <returns>The difference between <paramref name="a"/> and <paramref name="b"/>.</returns>
    public static RgbColor operator -(RgbColor a, RgbColor b)
        => _applyBinaryOpUnchecked((x, y) => x - y, a, b);
    /// <summaryBase>Multiplies <paramref name="a"/> and <paramref name="b"/> componentwise</summaryBase>
    /// <summary>
    /// <inheritdoc cref="operator *(RgbColor, RgbColor)" path="/summaryBase"/><inheritdoc
    /// cref="RgbColor" path="/uncheckedSummary"/>
    /// </summary>
    /// <param name="a">The first color to multiply.</param>
    /// <param name="b">The second color to multiply.</param>
    /// <returns>The product of <paramref name="a"/> and <paramref name="b"/>.</returns>
    public static RgbColor operator *(RgbColor a, RgbColor b)
        => _applyBinaryOpUnchecked((x, y) => x * y, a, b);
    /// <summaryBase>Divides <paramref name="a"/> by <paramref name="b"/> componentwise</summaryBase>
    /// <summary>
    /// <inheritdoc cref="operator /(RgbColor, RgbColor)" path="/summaryBase"/><inheritdoc
    /// cref="RgbColor" path="/uncheckedSummary"/>
    /// </summary>
    /// <param name="a">The dividend.</param>
    /// <param name="b">The divisor.</param>
    /// <returns>The quotient of <paramref name="a"/> and <paramref name="b"/>.</returns>
    public static RgbColor operator /(RgbColor a, RgbColor b)
        => _applyBinaryOpUnchecked((x, y) => x * y, a, b);
    /// <summaryBase>Adds <paramref name="addend"/> to each component of <paramref name="color"/></summaryBase>
    /// <summary>
    /// <inheritdoc cref="operator +(RgbColor, byte)" path="/summaryBase"/><inheritdoc
    /// cref="RgbColor" path="/uncheckedSummary"/>
    /// </summary>
    /// <param name="color">The color to be added to.</param>
    /// <param name="addend">The number to add to each component.</param>
    /// <returns>The result of adding <paramref name="addend"/> to <paramref name="color"/> componentwise.</returns>
    public static RgbColor operator +(RgbColor color, byte addend)
        => _applyUnaryOpUnchecked(x => x + addend, color);
    /// <summaryBase>Subtracts <paramref name="addend"/> to each component of <paramref name="color"/></summaryBase>
    /// <summary>
    /// <inheritdoc cref="operator -(RgbColor, byte)" path="/summaryBase"/><inheritdoc
    /// cref="RgbColor" path="/uncheckedSummary"/>
    /// </summary>
    /// <param name="color">The color to be added to.</param>
    /// <param name="addend">The number to add to each component.</param>
    /// <returns>The result of adding <paramref name="addend"/> to <paramref name="color"/> componentwise.</returns>
    public static RgbColor operator -(RgbColor color, byte addend)
        => _applyUnaryOpUnchecked(x => x + addend, color);
    /// <summaryBase>
    /// Multiplies each component of the specified <paramref name="color"/> by the specified
    /// <paramref name="coefficient"/>
    /// </summaryBase>
    /// <summary>
    /// <inheritdoc cref="operator *(RgbColor, float)" path="/summaryBase"/><inheritdoc
    /// cref="RgbColor" path="/uncheckedSummary"/>
    /// </summary>
    /// <param name="color">The color to be multiplied.</param>
    /// <param name="coefficient">The coefficient by which the color will be multiplied.</param>
    /// <returns>The product of <paramref name="color"/> and <paramref name="coefficient"/>.</returns>
    public static RgbColor operator *(RgbColor color, float coefficient)
        => _applyUnaryOpUnchecked(x => x * coefficient, color);
    /// <inheritdoc cref="operator *(RgbColor, float)"/>
    public static RgbColor operator *(RgbColor color, double coefficient)
        => _applyUnaryOpUnchecked(x => x * coefficient, color);
    /// <inheritdoc cref="operator *(RgbColor, float)"/>
    public static RgbColor operator *(RgbColor color, decimal coefficient)
        => _applyUnaryOpUnchecked(x => x * coefficient, color);
}