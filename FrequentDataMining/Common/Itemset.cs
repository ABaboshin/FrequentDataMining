// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;

namespace FrequentDataMining.Common
{
    public class Itemset<T> : IComparable<Itemset<T>> where T : IComparable<T>, IEquatable<T>
    {
        public List<T> Value { get; set; }
        public int Support { get; set; }

        public int CompareTo(Itemset<T> other)
        {
            return Value.Compare(other.Value);
        }

        public override string ToString()
        {
            return string.Join("", Value.Select(n => n.ToString())) + " " + Support;
        }
    }
}
