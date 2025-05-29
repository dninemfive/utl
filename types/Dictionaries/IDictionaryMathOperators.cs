using System.Numerics;

namespace d9.utl.types;
/// <summary>
/// Interface describing a dictionary type whose instances can perform math operations with constants
/// of their values' (<see cref="INumber{TSelf}">numeric</see>) type and with other dictionaries of
/// the same type.
/// dictionaries of the same type
/// </summary>
/// <typeparam name="K">The type of the keys of this dictionary.</typeparam>
/// <typeparam name="V">The type of the values of this dictionary.</typeparam>
public interface IDictionaryMathOperators<K, V>
    : IReadOnlyDictionary<K, V>
    where K : notnull
    where V : INumber<V>
{
    /// <summary>
    /// Adds a <paramref name="addend"/> to all values of this <paramref name="dictionary"/>..
    /// </summary>
    /// <param name="dictionary">The dictionary whose values to add to.</param>
    /// <param name="addend">The constant to add to all values of the dictionary.</param>
    /// <returns>
    /// A <b>copy</b> of the <paramref name="dictionary"/> where all values are the sum of the input value
    /// and <paramref name="addend"/>.
    /// </returns>
    public static IDictionaryMathOperators<K, V> operator +(IDictionaryMathOperators<K, V> dictionary, V addend)
        => (IDictionaryMathOperators<K, V>)dictionary.Add(addend);
    /// <summary>
    /// Subtracts a <paramref name="subtrahend"/> from all values of this <paramref name="dictionary"/>.
    /// </summary>
    /// <param name="dictionary">The dictionary whose values to subtract from.</param>
    /// <param name="subtrahend">The constant to subtract from all values of the dictionary.</param>
    /// <returns>
    /// A <b>copy</b> of the <paramref name="dictionary"/> where all values are the result of
    /// subtracting <paramref name="subtrahend"/> from the input value.
    /// </returns>
    public static IDictionaryMathOperators<K, V> operator -(IDictionaryMathOperators<K, V> dictionary, V subtrahend)
        => (IDictionaryMathOperators<K, V>)dictionary.Subtract(subtrahend);
    /// <summary>
    /// Multiplies all values of this <paramref name="dictionary"/> by a specified <paramref name="factor"/>.
    /// </summary>
    /// <param name="dictionary">The dictionary whose values to multiply.</param>
    /// <param name="factor">The value by which to multiply all values in the <paramref name="dictionary"/>.</param>
    /// <returns>
    /// A <b>copy</b> of the <paramref name="dictionary"/> where all values are the result of
    /// subtracting <paramref name="factor"/> from the input value.
    /// </returns>
    public static IDictionaryMathOperators<K, V> operator *(IDictionaryMathOperators<K, V> dictionary, V factor)
        => (IDictionaryMathOperators<K, V>)dictionary.Multiply(factor);
    /// <summary>
    /// Multiplies all values of this <paramref name="dictionary"/> by a specified <paramref name="divisor"/>.
    /// </summary>
    /// <param name="dictionary">The dictionary whose values to multiply.</param>
    /// <param name="divisor">The value by which to divide all values in the <paramref name="dictionary"/>.</param>
    /// <returns>
    /// A <b>copy</b> of the <paramref name="dictionary"/> where all values are the result of
    /// subtracting <paramref name="divisor"/> from the input value.
    /// </returns>
    public static IDictionaryMathOperators<K, V> operator /(IDictionaryMathOperators<K, V> dictionary, V divisor)
        => (IDictionaryMathOperators<K, V>)dictionary.Divide(divisor);
    /// <summary>
    /// Adds two dictionaries together.
    /// </summary>
    /// <param name="left">The first dictionary to add.</param>
    /// <param name="right">The second dictionary to add.</param>
    /// <returns>
    /// A new dictionary whose keys are the union of the keys of those in <paramref name="left"/>
    /// and <paramref name="right"/> and whose corresponding values are the sum of those in
    /// <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static IDictionaryMathOperators<K, V> operator +(IDictionaryMathOperators<K, V> left, IDictionaryMathOperators<K, V> right)
        => (IDictionaryMathOperators<K, V>)left.Add(right);
    /// <summary>
    /// Subtracts one dictionary from another.
    /// </summary>
    /// <param name="left">The dictionary from which to subtract.</param>
    /// <param name="right">The dictionary by which to subtract.</param>
    /// <returns>
    /// A new dictionary whose keys are the union of the keys of those in <paramref name="left"/>
    /// and <paramref name="right"/> and whose corresponding values are the difference between those 
    /// in <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static IDictionaryMathOperators<K, V> operator -(IDictionaryMathOperators<K, V> left, IDictionaryMathOperators<K, V> right)
        => (IDictionaryMathOperators<K, V>)left.Subtract(right);
    /// <summary>
    /// Multiplies one dictionary by another.
    /// </summary>
    /// <param name="left">The first dictionary to multiply.</param>
    /// <param name="right">The second dictionary to multiply.</param>
    /// <returns>
    /// A new dictionary whose keys are the union of the keys of those in <paramref name="left"/>
    /// and <paramref name="right"/> and whose corresponding values are the product of those 
    /// in <paramref name="left"/> and <paramref name="right"/>.
    /// </returns>
    public static IDictionaryMathOperators<K, V> operator *(IDictionaryMathOperators<K, V> left, IDictionaryMathOperators<K, V> right)
        => (IDictionaryMathOperators<K, V>)left.Multiply(right);
    /// <summary>
    /// Divides one dictionary by another.
    /// </summary>
    /// <param name="left">The dictionary to be divided.</param>
    /// <param name="right">The dictionary by which to divide.</param>
    /// <returns>
    /// A new dictionary whose keys are the union of the keys of those in <paramref name="left"/>
    /// and <paramref name="right"/> and whose corresponding values are the dividend of those 
    /// in <paramref name="left"/> divided by those in <paramref name="right"/>.
    /// </returns>
    public static IDictionaryMathOperators<K, V> operator /(IDictionaryMathOperators<K, V> left, IDictionaryMathOperators<K, V> right)
        => (IDictionaryMathOperators<K, V>)left.Divide(right);
}
