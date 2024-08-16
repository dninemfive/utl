using System.Numerics;

namespace d9.utl;
public readonly partial struct RgbColor
{
    private static RgbColor _applyBinaryOpChecked(Func<byte, byte, int> op, RgbColor a, RgbColor b)
        => _applyBinaryOp(op, a, b, byte.CreateChecked);
    public static RgbColor operator checked +(RgbColor a, RgbColor b)
        => _applyBinaryOpChecked((x, y) => x + y, a, b);
    public static RgbColor operator checked -(RgbColor a, RgbColor b)
        => _applyBinaryOpChecked((x, y) => x - y, a, b);
    public static RgbColor operator checked *(RgbColor a, RgbColor b)
        => _applyBinaryOpChecked((x, y) => x * y, a, b);
    public static RgbColor operator checked /(RgbColor a, RgbColor b)
        => _applyBinaryOpChecked((x, y) => x * y, a, b);
    private static RgbColor _applyUnaryOpChecked<T>(Func<byte, T> op, RgbColor a)
        where T : INumberBase<T>
        => _applyUnaryOp(op, a, byte.CreateChecked);    
    public static RgbColor operator checked *(RgbColor color, float coefficient)
        => _applyUnaryOpChecked(x => x * coefficient, color);
    public static RgbColor operator checked *(RgbColor color, double coefficient)
        => _applyUnaryOpChecked(x => x * coefficient, color);
}
