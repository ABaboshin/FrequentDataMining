// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;

namespace FrequentDataMining.Common
{
    public class TypeRegister
    {
        static Dictionary<Type, object> Compares = new Dictionary<Type, object>();

        /// <summary>
        /// Type registration
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="comparer">compareTo</param>
        public static void Register<T>(Func<T, T, int> comparer)
        {
            if (Compares.ContainsKey(typeof(T)))
            {
                Compares.Remove(typeof(T));
            }


            Compares.Add(typeof(T), comparer);
        }

        internal static Func<T, T, int> GetComparer<T>()
        {
            if (Compares.ContainsKey(typeof(T)))
            {
                return (Func<T, T, int>)Compares[typeof(T)];
            }

            return null;
        }

        internal static Func<List<T>, List<T>> GetSorter<T>()
        {
            return list =>
            {
                list.Sort((a, b) => GetComparer<T>()(a, b));
                return list;
            };
        }
    }
}
