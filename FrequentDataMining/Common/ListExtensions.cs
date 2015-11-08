// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;

namespace FrequentDataMining.Common
{
    internal static class ListExtensions
    {
        public static bool Equal<T>(this List<T> a, List<T> b) where T : IEquatable<T>, IComparable<T>
        {
            if (a.Count() != b.Count())
            {
                return false;
            }

            var n1 = new Sorter<T>().Sort(a).ToList();
            var n2 = new Sorter<T>().Sort(b).ToList();

            for (var i = 0; i < n1.Count(); i++)
            {
                if (!n1[i].Equals(n2[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static int Compare<T>(this List<T> a, List<T> b) where T : IEquatable<T>, IComparable<T>
        {
            var n1 = new Sorter<T>().Sort(a).ToList();
            var n2 = new Sorter<T>().Sort(b).ToList();

            for (int i = 0; i < Math.Min(n1.Count(), n2.Count()); i++)
            {
                var cmp = n1[i].CompareTo(n2[i]);
                if (cmp != 0)
                {
                    return cmp;
                }
            }

            return n1.Count().CompareTo(n2.Count());
        }

        public static List<T> Allocate<T>(int count) where  T : class
        {
            var list = new List<T>();
            for (var i = 0; i < count; i++)
            {
                list.Add(null);
            }

            return list;
        }
    }
}
