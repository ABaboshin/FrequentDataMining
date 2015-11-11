// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;

namespace FrequentDataMining.Common
{
    internal static class ListExtensions
    {
        public static bool Equal<T>(this List<T> a, List<T> b)
        {
            if (a.Count() != b.Count())
            {
                return false;
            }

            var sorter = TypeRegister.GetSorter<T>();

            var n1 = sorter(a).ToList();
            var n2 = sorter(b).ToList();

            var compareTo = TypeRegister.GetComparer<T>();
            for (var i = 0; i < n1.Count(); i++)
            {
                if (compareTo(n1[i], n2[i]) != 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static int Compare<T>(this List<T> a, List<T> b)
        {
            var sorter = TypeRegister.GetSorter<T>();
            var n1 = sorter(a).ToList();
            var n2 = sorter(b).ToList();

            var compareTo = TypeRegister.GetComparer<T>();
            for (var i = 0; i < Math.Min(n1.Count(), n2.Count()); i++)
            {
                var cmp = compareTo(n1[i], n2[i]);
                if (cmp != 0)
                {
                    return cmp;
                }
            }

            return n1.Count().CompareTo(n2.Count());
        }

        public static List<T> Allocate<T>(int count)
        {
            var ar = new T[count];
            return ar.ToList();
        }
    }
}
