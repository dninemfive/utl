using System.Collections;
namespace d9.utl;
/// <summary>
/// Represents the idea of a closed loop of values which can be iterated along.
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
    public Cycle(params T[] items) : this(items.ToList()) { }
    public Cycle(IEnumerable<T> items)
    {
        _list = items.ToList();
    }
    public T CurrentItem => _list[Index];
    public T NextItem()
    {
        Utils.Log($"NextItem({Index})");
        Index++;
        Utils.Log($"\t-> {Index}");
        return CurrentItem;
    }
    public T PreviousItem()
    {
        Utils.Log($"PrevItem({Index})");
        Index--;
        Utils.Log($"\t-> {Index}");
        return CurrentItem;
    }
    /// <summary>
    /// Inserts the specified <paramref name="item"/> before the <see cref="CurrentItem"/>.
    /// </summary>
    /// <param name="item">The item to insert.</param>
    public void Prepend(T item)
    {
        _list.Insert(Offset(-1), item);
    }
    /// <summary>
    /// Gets the enumerator for this cycle, starting at the <see cref="CurrentItem">current item</see>
    /// and continuing until it has looped back around to the previous element.
    /// </summary>
    /// <returns>Every item in the cycle exactly once, starting at the <see cref="CurrentItem">current item</see>.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        T[] array = new T[_list.Count];
        for(int i = 0; i < _list.Count; i++)
        {
            array[i] = _list[Offset(i)];
        }
        return new GenericEnumerator<T>(array);
    }
    /// <inheritdoc cref="GetEnumerator"/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        T[] array = new T[_list.Count];
        for (int i = 0; i < _list.Count; i++)
        {
            array[i] = _list[Offset(i)];
        }
        return new GenericEnumerator<T>(array);
    }
    /// <summary>
    /// Gets the item at the specified <paramref name="offset"/> in the sequence.
    /// </summary>
    /// <param name="offset">The index of the item to select, relative to the current item.</param>
    /// <returns>The item at the specified position.</returns>
    public T this[int offset] => _list[Offset(offset)];
    public void Clear() => _list.Clear();
}
