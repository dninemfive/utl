using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl.types;
public class DefaultDictionary<K, V>
    : BaseDictionary<K, V>
    where K : notnull
{
    private readonly Func<K, V> _defaultFrom;
    public DefaultDictionary(Func<K, V> defaultFunction) 
        : base()
        => _defaultFrom = defaultFunction;
    public DefaultDictionary(IEnumerable<KeyValuePair<K, V>> values, Func<K, V> defaultFunction)
        : base(values)
        => _defaultFrom = defaultFunction;
    public DefaultDictionary(IEnumerable<(K key, V value)> values, Func<K, V> defaultFunction)
        : base(values)
        => _defaultFrom = defaultFunction;
    protected override V GetDefaultValue(K key)
        => _defaultFrom(key);
}
