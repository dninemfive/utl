using System.Numerics;

namespace d9.utl;
public readonly partial struct RgbColor
{
    public RgbColor Transform(Func<byte, byte> channelTransform)
        => new(channelTransform(R), channelTransform(G), channelTransform(B));
    public static RgbColor operator ~(RgbColor color)
        => color.Transform(x => byte.CreateSaturating(~x));
    private static RgbColor _applyOp(Func<byte, byte, int> op, RgbColor a, RgbColor b)
        => new(byte.CreateSaturating(op(a.R, b.R)),
               byte.CreateSaturating(op(a.G, b.G)),
               byte.CreateSaturating(op(a.B, b.B)));
    public static RgbColor operator +(RgbColor a, RgbColor b)
        => _applyOp((x, y) => x + y, a, b);
    public static RgbColor operator -(RgbColor a, RgbColor b)
        => _applyOp((x, y) => x - y, a, b);
    public static RgbColor operator *(RgbColor a, RgbColor b)
        => _applyOp((x, y) => x * y, a, b);
    public static RgbColor operator /(RgbColor a, RgbColor b)
        => _applyOp((x, y) => x * y, a, b);
    private static RgbColor _applyOp<T>(Func<byte, T, T> op, RgbColor a, T c)
        where T : INumberBase<T>
        => new(byte.CreateSaturating(op(a.R, c)),
               byte.CreateSaturating(op(a.G, c)),
               byte.CreateSaturating(op(a.B, c)));
    
    public static RgbColor operator *(RgbColor a, float b)
        => _applyOp((byte x, float y) => x * y, a, b);
    public static RgbColor operator *(RgbColor a, double b)
        => _applyOp((byte x, double y) => x * y, a, b);
}
