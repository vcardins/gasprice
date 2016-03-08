using System;
using System.Collections.Generic;
using System.Linq;

namespace GasPrice.Core.Common.Extensions
{
    public static class EnumerableExtensions
    {
         public static void ForEach<T>(this IEnumerable<T> items, Action<T> action) 
         {
             foreach (var item in items)
             {
                 action(item);
             }
         }

         public static string ToString<T>(this IEnumerable<T> l, string separator)
         {
             return "[" + String.Join(separator, l.Select(i => i.ToString()).ToArray()) + "]";
         }
    }
}

