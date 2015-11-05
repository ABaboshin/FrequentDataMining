// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;

namespace FrequentDataMining.Apriori
{
    public class FrequentItemsCollection<T> 
        where T : IComparable<T>, IEquatable<T>
    {
        public FrequentItemsCollection()
        {
            Values = new List<Item<T>>();
        }

        public List<Item<T>> Values { get; set; }

        public void ConcatItems(List<Item<T>> items) {
            Values = Values.Concat(items).ToList();
        }

        public int Count() {
            return Values.Count();
        }

        public Item<T> this[List<T> name] {
            get {
                return Values.Where(v => v.Name.Equal(name)).FirstOrDefault();
            }
        }

        public Item<T> this[int idx] {
            get {
                return Values[idx];
            }
        }
    }
}
