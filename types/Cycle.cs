using System.Collections;

namespace d9.utl;
/// <summary>
/// Represents the idea of a closed loop of values which can be iterated along in either direction.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Cycle<T> : IEnumerable<T>
{
    private readonly List<T> _list = new();
    private int _index = 0;
    private int Index
    {
        get => _index;
        set => _index = Clamp(value);
    }
    private int Clamp(int index) => (index < 0, index >= _list.Count) switch
    {
        (true, false) => _list.Count - 1,
        (false, true) => 0,
        _ => index
    };
    private int Offset(int i) => Clamp(Index + i);
    /// <summary>
    /// Creates a new Cycle from the corresponding <paramref name="items"/>.
    /// </summary>
    /// <param name="items">The items from which to create the Cycle.</param>
    public Cycle(params T[] items) : this(items.AsEnumerable()) { }
    /// <inheritdoc cref="Cycle{T}.Cycle(T[])"/>
    public Cycle(IEnumerable<T> items)
    {
        _list = items.ToList();
    }
    /// <summary>
    /// Gets the current item in the cycle.
    /// </summary>
    public T CurrentItem => _list[Index];
    /// <summary>
    /// Moves to the next item in the cycle.
    /// </summary>
    /// <returns>The new current item.</returns>
    public T NextItem()
    {
        Index++;
        return CurrentItem;
    }
    /// <summary>
    /// Moves to the previous item in the cycle.
    /// </summary>
    /// <returns><inheritdoc cref="NextItem" path="/returns"/></returns>
    public T PreviousItem()
    {
        Index--;
        return CurrentItem;
    }
    /// <summary>
    /// Inserts the specified <paramref name="item"/> before the <see cref="CurrentItem">current item</see>.
    /// </summary>
    /// <param name="item">The item to insert.</param>
    public void Prepend(T item)
        => _list.Insert(Offset(-1), item);
    /// <summary>
    /// Inserts the specified <paramref name="item"/> after the <see cref="CurrentItem">current item</see>.
    /// </summary>
    /// <param name="item"><inheritdoc cref="Prepend(T)" path="/param[@name='item'"/></param>
    public void Append(T item)
        => _list.Insert(Offset(1), item);
    /// <summary>
    /// Gets the enumerator for this cycle, starting at the <see cref="CurrentItem">current
    /// item</see> and continuing until it has looped back around to the previous element.
    /// </summary>
    /// <returns>
    /// Every item in the cycle exactly once, starting at the <see cref="CurrentItem">current item</see>.
    /// </returns>
    public IEnumerator<T> GetEnumerator()
    {
        T[] array = new T[_list.Count];
        for (int i = 0; i < _list.Count; i++)
        {
            array[i] = _list[Offset(i)];
        }
        return new GenericEnumerator<T>(array);
    }
    /// <inheritdoc cref="GetEnumerator"/>
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    /// <summary>
    /// Gets the item at the specified <paramref name="offset"/> in the sequence.
    /// </summary>
    /// <param name="offset">
    /// The index of the item to select, relative to the <see cref="CurrentItem">current item</see>.
    /// </param>
    /// <returns>The item at the specified position.</returns>
    public T this[int offset] => _list[Offset(offset)];
    /// <summary>
    /// Clears the cycle of all items.
    /// </summary>
    public void Clear() => _list.Clear();
    /// <summary>
    /// How many items are in this cycle.
    /// </summary>
    public int Count => _list.Count;
}