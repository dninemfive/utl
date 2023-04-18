using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utl
{
    /// <summary>
    /// Useful global variables.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Normal apostrophes plus the weird ones that iPhones automatically replace the normal ones with.
        /// </summary>
        public static readonly char[] FancyApostrophes = new[] { '\'', '‘', '’' };
        /// <summary>
        /// Normal quotation marks plus the weird ones that iPhones automatically replace the normal ones with.
        /// </summary>
        public static readonly char[] FancyQuotes = new[] { '"', '“', '”' };
        /// <summary>
        /// The null character, traditionally used for ending strings.
        /// </summary>
        public const char NullCharacter = (char)0;
    }
}
