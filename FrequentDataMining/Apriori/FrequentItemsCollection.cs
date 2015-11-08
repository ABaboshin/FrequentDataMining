// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;
using FrequentDataMining.Common;

namespace FrequentDataMining.Apriori
{
    public class FrequentItemsCollection<T> 
        where T : IComparable<T>, IEquatable<T>
    {
        public FrequentItemsCollection()
        {
            Values = new List<Itemset<T>>();
        }

        public List<Itemset<T>> Values { get; set; }

        public void ConcatItems(List<Itemset<T>> items) {
            Values = Values.Concat(items).ToList();
        }

        public int Count() {
            return Values.Count();
        }

        public Itemset<T> this[List<T> name] {
            get {
                return Values.FirstOrDefault(v => v.Value.Equal(name));
            }
        }

        public Itemset<T> this[int idx] => Values[idx];
    }
}
