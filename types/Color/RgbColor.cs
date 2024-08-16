using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace d9.utl;
public readonly partial struct RgbColor(byte r, byte g, byte b)
    : IParsable<RgbColor>
{
    public readonly byte R = r, G = g, B = b;
    public static RgbColor Parse(string s, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out RgbColor result)
    {
        result = default;
        if (s is null)
            return false;
        if (!_hexRegex.IsMatch(s))
            throw new Exception($"Cannot parse RgbColor from string {s}: unrecognized format!");
        int offset = s.StartsWith('#') ? 1 : 0;
        // this code is cursed but i wanted to see if it works lol
        if (!(byte.TryParse(s[offset++..offset++], out byte r)
            & byte.TryParse(s[offset++..offset++], out byte g)
            & byte.TryParse(s[offset++..offset++], out byte b)))
        {
            result = new(r, g, b);
            return true;
        }
        return false;

    }
    private static Regex _hexRegex = GenerateHexRegex();
    public void Deconstruct(out byte r, out byte g, out byte b)
    {
        r = R;
        g = G;
        b = B;
    }
    public override string ToString()
        => $"#{R.ToHex()}{G.ToHex()}{B.ToHex()}";
    public static implicit operator string(RgbColor color)
        => color.ToString();
    public static implicit operator RgbColor((byte r, byte g, byte b) tuple)
        => new(tuple.r, tuple.g, tuple.b);

    [GeneratedRegex(@"#?\d{6}")]
    private static partial Regex GenerateHexRegex();
}