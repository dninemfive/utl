using System.Numerics;

namespace d9.utl;
public readonly partial struct RgbColor
{
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
    /// <summary>
    /// Transforms this RGB color by applying the specified <paramref name="channelTransform"/> to each component.
    /// </summary>
    /// <typeparam name="T">The type of the result of the transform. The result will be converted back to a <see langword="byte"/>.</typeparam>
    /// <param name="channelTransform">The transform to apply to the R, G, and B channels individually.</param>
    /// <param name="checked">If <see langword="true"/>, an exception will be thrown if the result of any of the transforms is outside the valid range of a <see langword="byte"/>; otherwise, values will be clamped to that range.</param>
    /// <returns>The transformed color.</returns>
    public RgbColor Transform<T>(Func<byte, T> channelTransform, bool @checked)
        where T : INumberBase<T>
        => _applyUnaryOp(channelTransform, this, @checked ? byte.CreateChecked : byte.CreateSaturating);
    /// <summary>
    /// Inverts this color componentwise.
    /// </summary>
    /// <param name="color">The color to be inverted.</param>
    /// <returns>The inverse of the specified color.</returns>
    public static RgbColor operator ~(RgbColor color)
        => _applyUnaryOp(x => ~x, color, byte.CreateTruncating);
}
