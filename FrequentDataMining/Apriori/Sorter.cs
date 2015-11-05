// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;

namespace FrequentDataMining.Apriori
{
    class Sorter<T> where T : IComparable<T>
    {
        public List<T> Sort(List<T> token) {
            var tmp = token.ToList();
            tmp.Sort();
            return tmp;
        }
    }
}
