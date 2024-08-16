using System.Numerics;

namespace d9.utl;
public readonly partial struct RgbColor
{
    private static RgbColor _applyBinaryOpUnchecked(Func<byte, byte, int> op, RgbColor a, RgbColor b)
        => _applyBinaryOp(op, a, b, byte.CreateSaturating);
    private static RgbColor _applyUnaryOpUnchecked<T>(Func<byte, T> op, RgbColor color)
        where T : INumberBase<T>
        => _applyUnaryOp(op, color, byte.CreateSaturating);
    public static RgbColor operator +(RgbColor a, RgbColor b)
        => _applyBinaryOpUnchecked((x, y) => x + y, a, b);
    public static RgbColor operator -(RgbColor a, RgbColor b)
        => _applyBinaryOpUnchecked((x, y) => x - y, a, b);
    public static RgbColor operator *(RgbColor a, RgbColor b)
        => _applyBinaryOpUnchecked((x, y) => x * y, a, b);
    public static RgbColor operator /(RgbColor a, RgbColor b)
        => _applyBinaryOpUnchecked((x, y) => x * y, a, b);
    public static RgbColor operator ~(RgbColor color)
        => _applyUnaryOpUnchecked(x => ~x, color);
    public static RgbColor operator +(RgbColor color, byte addend)
        => _applyUnaryOpUnchecked(x => x + addend, color);
    public static RgbColor operator *(RgbColor color, float coefficient)
        => _applyUnaryOpUnchecked(x => x * coefficient, color);
    public static RgbColor operator *(RgbColor color, double coefficient)
        => _applyUnaryOpUnchecked(x => x * coefficient, color);
    public static RgbColor operator *(RgbColor color, decimal coefficient)
        => _applyUnaryOpUnchecked(x => x * coefficient, color);
}
