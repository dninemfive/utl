using System.Collections;
namespace d9.utl;
/// <summary>
/// A generic but instatiatable implementation of <see cref="IEnumerator{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the items to enumerate.</typeparam>
public class GenericEnumerator<T> : IEnumerator<T>, IDisposable
{
    private int _index = -1;
    private readonly T[] _items;
    /// <summary>
    /// Creates a GenericEnumerator from the specified <paramref name="items"/>.
    /// </summary>
    /// <param name="items">The items with which to create the enumerator.</param>
    public GenericEnumerator(params T[] items)
    {
        _items = items;
    }
    /// <inheritdoc cref="GenericEnumerator{T}.GenericEnumerator(T[])"/>
    public GenericEnumerator(IEnumerable<T> items) : this(items.ToArray()) { }
    /// <summary>
    /// The current item in the enumerator.
    /// </summary>
    public T Current
    {
        get
        {
            try
            {
                return _items[_index];
            } 
            catch
            {
                throw new Exception($"GenericEnumerator<{typeof(T).Name}>.Current is undefined when _index = {_index}!");
            }
        }
    }
    object IEnumerator.Current => Current!;
    /// <summary>
    /// Moves the current index of the enumerator to the next location.
    /// </summary>
    /// <returns>Whether the new location is within the bounds of the internal array.</returns>
    public bool MoveNext()
    {
        _index++;
        return _index >= 0 && _index < _items.Length;
    }
    /// <summary>
    /// Resets the index such that the enumerator will yield the first element in the sequence after <see cref="MoveNext"/> is called.
    /// </summary>
    public void Reset() => _index = -1;
    /// <summary>
    /// Not implemented, as there are no additional resources to dispose. See 
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerator-1?view=net-7.0#notes-to-implementers">
    /// here</see> for more information.
    /// </summary>
    #pragma warning disable CA1816 // "call GC.SuppressFinalize()": derived classes should implement Dispose() if needed
    public void Dispose() { }
    #pragma warning restore CA1816
}
