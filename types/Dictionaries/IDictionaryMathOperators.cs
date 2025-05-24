using System.Numerics;

namespace d9.utl.types.Dictionaries;
public interface IDictionaryMathOperators<K, V>
    : IReadOnlyDictionary<K, V>
    where K : notnull
    where V : INumber<V>
{
    public static IDictionaryMathOperators<K, V> operator +(IDictionaryMathOperators<K, V> dict, V c)
        => (IDictionaryMathOperators<K, V>)dict.Add(c);
    public static IDictionaryMathOperators<K, V> operator -(IDictionaryMathOperators<K, V> dict, V c)
        => (IDictionaryMathOperators<K, V>)dict.Subtract(c);
    public static IDictionaryMathOperators<K, V> operator *(IDictionaryMathOperators<K, V> dict, V c)
        => (IDictionaryMathOperators<K, V>)dict.Multiply(c);
    public static IDictionaryMathOperators<K, V> operator /(IDictionaryMathOperators<K, V> dict, V c)
        => (IDictionaryMathOperators<K, V>)dict.Divide(c);
    public static IDictionaryMathOperators<K, V> operator +(IDictionaryMathOperators<K, V> a, IDictionaryMathOperators<K, V> b)
        => (IDictionaryMathOperators<K, V>)a.Add(b);
    public static IDictionaryMathOperators<K, V> operator -(IDictionaryMathOperators<K, V> a, IDictionaryMathOperators<K, V> b)
        => (IDictionaryMathOperators<K, V>)a.Subtract(b);
    public static IDictionaryMathOperators<K, V> operator *(IDictionaryMathOperators<K, V> a, IDictionaryMathOperators<K, V> b)
        => (IDictionaryMathOperators<K, V>)a.Multiply(b);
    public static IDictionaryMathOperators<K, V> operator /(IDictionaryMathOperators<K, V> a, IDictionaryMathOperators<K, V> b)
        => (IDictionaryMathOperators<K, V>)a.Divide(b);
}
