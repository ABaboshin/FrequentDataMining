// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;

namespace FrequentDataMining.Apriori
{
    public class Item<T> : IComparable<Item<T>> where T : IComparable<T>, IEquatable<T>
    {
        public List<T> Name { get; set; }
        public double Support { get; set; }

        public int CompareTo(Item<T> other)
        {
            return Name.Compare(other.Name);
        }

        public override string ToString()
        {
            return string.Join("", Name.Select(n => n.ToString())) + " " + Support;
        }
    }
}
