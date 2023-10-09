using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl;
public class GenericEnumerator<T> : IEnumerator<T>, IDisposable
{
    private int _index = -1;
    private T[] _items;
    public GenericEnumerator(params T[] items)
    {
        _items = items;
    }
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
    object IEnumerator.Current => throw new NotImplementedException();

    public bool MoveNext()
    {
        _index++;
        return _index >= 0 && _index < _items.Length;
    }
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
