using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl
{
    /// <summary>
    /// Dictionary variant which simplifies multiple keys corresponding to the same value.
    /// </summary>
    /// <typeparam name="K">The type of the key. Must be <see langword="notnull"/>.</typeparam>
    /// <typeparam name="V"></typeparam>
    public class AliasDictionary<K, V> where K : notnull
    {
        private readonly Dictionary<K, K> Aliases = new();
        private readonly Dictionary<K, V?> Values = new();
        /// <summary>
        /// Gets or sets the value with the alias <c>key</c>.
        /// </summary>
        /// <param name="key">The alias of the variable to get or set.</param>
        /// <returns>If getting, the value ultimately corresponding to <c>alias</c>, or <see langword="null"/> otherwise.</returns>
        public V? this[K key]
        {
            get
            {
                if (!ContainsKey(key)) return default;
                return Values[Aliases[key]];
            }
            set
            {
                Values[Aliases[key]] = value;
            }
        }
        /// <summary>
        /// Tells whether the dictionary contains the specified alias already.
        /// </summary>
        /// <param name="key">The key whose presence to check for.</param>
        /// <returns><see langword="true"/> if the alias is present, or <see langword="false"/> otherwise.</returns>
        public bool ContainsKey(K key) => Aliases.ContainsKey(key);
        /// <summary>
        /// Adds the alias(es) to the dictionary with a <see langword="null"/> value.
        /// </summary>
        /// <param name="key">The first alias to add.</param>
        /// <param name="aliases"></param>
        public void Add(K key, params K[] aliases) => Add(key, value: default, aliases);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="aliases"></param>
        public void Add(K key, V? value, params K[] aliases) {
            Values[key] = value;
            Aliases[key] = key;
            foreach (K alias in aliases) Aliases[key] = alias;
        }
    }
}
