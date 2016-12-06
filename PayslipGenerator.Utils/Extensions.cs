using System;
using System.Collections.Generic;

namespace PayslipGenerator.Utils
{
    /// <summary>
    /// combination of some extension methods
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// makes it easy to traverse a collection of items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="action"></param>
        public static void Foreach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }
    }
}