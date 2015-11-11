// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;

namespace FrequentDataMining.Common
{
    public class TypeRegister
    {
        static Dictionary<Type, object> Compares = new Dictionary<Type, object>();
        static Dictionary<Type, object> Sorters = new Dictionary<Type, object>();

        /// <summary>
        /// Type registration
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="comparer">compareTo</param>
        /// <param name="sorter">sort function</param>
        public static void Register<T>(Func<T, T, int> comparer, Func<IEnumerable<T>, IEnumerable<T>> sorter)
        {
            if (Compares.ContainsKey(typeof(T)))
            {
                Compares.Remove(typeof(T));
            }

            if (Sorters.ContainsKey(typeof(T)))
            {
                Sorters.Remove(typeof(T));
            }

            Compares.Add(typeof(T), comparer);
            Sorters.Add(typeof(T), sorter);
        }

        internal static Func<T, T, int> GetComparer<T>()
        {
            if (Compares.ContainsKey(typeof(T)))
            {
                return (Func<T, T, int>)Compares[typeof(T)];
            }

            return null;
        }

        internal static Func<IEnumerable<T>, IEnumerable<T>> GetSorter<T>()
        {
            if (Sorters.ContainsKey(typeof(T)))
            {
                return (Func<IEnumerable<T>, IEnumerable<T>>)Sorters[typeof(T)];
            }

            return null;
        }
    }
}
