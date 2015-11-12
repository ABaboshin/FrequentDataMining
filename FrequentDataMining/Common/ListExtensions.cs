// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;

namespace FrequentDataMining.Common
{
    internal static class ListExtensions
    {
        public static bool Equal<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            if (a.Count() != b.Count())
            {
                return false;
            }

            return Compare(a, b) == 0;
        }

        public static int Compare<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            var sorter = TypeRegister.GetSorter<T>();
            var sortedA = sorter(a);
            var sortedB = sorter(b);

            var compareTo = TypeRegister.GetComparer<T>();
            for (var i = 0; i < Math.Min(sortedA.Count(), sortedB.Count()); i++)
            {
                var cmp = compareTo(sortedA.Skip(i).First(), sortedB.Skip(i).First());
                if (cmp != 0)
                {
                    return cmp;
                }
            }

            return sortedA.Count().CompareTo(sortedB.Count());
        }

        public static List<T> Allocate<T>(int count)
        {
            var ar = new T[count];
            return ar.ToList();
        }
    }
}
