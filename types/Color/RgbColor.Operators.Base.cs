using System.Numerics;

namespace d9.utl;
public readonly partial struct RgbColor
{
    public RgbColor Transform<T>(Func<byte, T> channelTransform, bool @checked)
        where T : INumberBase<T>
        => _applyUnaryOp(channelTransform, this, @checked ? byte.CreateChecked : byte.CreateSaturating);
    private static RgbColor _applyBinaryOp(Func<byte, byte, int> op, RgbColor a, RgbColor b, Func<int, byte> create)
    {
        return new(create(op(a.R, b.R)),
                   create(op(a.G, b.G)),
                   create(op(a.B, b.B)));
    }
    private static RgbColor _applyUnaryOp<T>(Func<byte, T> op, RgbColor a, Func<T, byte> create)
        where T : INumberBase<T>
        => new(create(op(a.R)),
               create(op(a.G)),
               create(op(a.B)));
}
