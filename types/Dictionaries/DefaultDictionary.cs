using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl.types;
public class DefaultDictionary<K, V> : IEnumerable<KeyValuePair<K, V>>, IDictionary<K, V>, IReadOnlyDictionary<K, V>
    where K : notnull
{
}
