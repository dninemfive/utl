﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.utils
{
    /// <summary>
    /// Utilities which convert objects to strings or perform operations on strings.
    /// </summary>
    public static class StringUtils
    {        
        /// <summary></summary>
        /// <typeparam name="T">The type of the objects to print.</typeparam>
        /// <param name="values">An enumerable holding the objects to print paired with the width of their respective columns.</param>
        /// <returns>A string corresponding to the objects <c>t</c> in order, with columns padded to <c>width</c>.</returns>
        public static string InColumns<T>(this IEnumerable<(T t, int width)> values)
        {
            string result = "";
            foreach ((T t, int width) in values)
            {
                result += t.PrintNullable().PadRight(width);
            }
            return result;
        }
        /// <typeparam name="T">The type of the objects to print.</typeparam>
        /// <param name="values">An enumerable holding the objects to print.</param>
        /// <param name="widths">An enumerable holding the widths of the columns, which will be applied in the same order as the objects.</param>
        /// <returns>A string corresponding to the <c>values</c> in order, in columns padded to their respective <c>widths</c>.</returns>
        public static string InColumns<T>(this IEnumerable<T> values, IEnumerable<int> widths) => InColumns(values.Zip(widths));
        /// <summary>
        /// Join a set of characters to a string.
        /// </summary>
        /// <param name="chars">The characters to join.</param>
        /// <returns>The specified characters, joined to a string.</returns>
        public static string JoinToString(this IEnumerable<char> chars) => chars.Select(x => "" + x).Aggregate((x, y) => x + y);
        /// <summary>
        /// Represents an enumerable in human-readable format.
        /// </summary>
        /// <typeparam name="T">The type the <c>enumerable</c> contains.</typeparam>
        /// <param name="enumerable">The enumerable to print.</param>
        /// <returns>A string of the format <c>[item1, item2, ... itemN]</c> representing the items in <c>enumerable</c>.</returns>
        public static string ListNotation<T>(this IEnumerable<T> enumerable)
            => $"[{enumerable.Select(x => x.PrintNullable()).Aggregate((a, b) => $"{a}, {b}")}]";
        /// <summary>
        /// Represents an object in human-readable format, even if it's <see langword="null"/>.
        /// </summary>
        /// <param name="obj">The object or <see langword="null"/> value to represent.</param>
        /// <param name="ifNull">The string to print if <c>obj</c> is null.</param>
        /// <returns>A string which is either <c>obj.ToString()</c>, if <c>obj</c> is not <see langword="null"/>, or <c>ifNull</c> otherwise.</returns>
        public static string PrintNullable(this object? obj, string ifNull = "(null (not Null))") => obj?.ToString() ?? ifNull;
        /// <summary>
        /// Repeats a character a specified number of times.
        /// </summary>
        /// <param name="c">The character to repeat.</param>
        /// <param name="times">How many of the character should be produced.</param>
        /// <returns>A string which is <c>times</c> instances of <c>c</c>.</returns>
        public static string Repeated(this char c, int times)
        {
            string result = "";
            for (int i = 0; i < times; i++) result += c;
            return result;
        }
        /// <summary>Removes a set of characters from a string.</summary>
        /// <param name="s">The string from which the characters will be removed.</param>
        /// <param name="chars">The characters to be removed.</param>
        /// <returns>A copy of <c>s</c> without any instances of the specified characters..</returns>
        public static string Without(this string s, IEnumerable<char> chars)
        {
            foreach (char c in chars) s = s.Replace("" + c, "");
            return s;
        }
    }
}
