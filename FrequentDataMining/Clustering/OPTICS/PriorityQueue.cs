// MIT License.
// (c) 2015, Andrey Baboshin

using System.Collections.Generic;
using System.Linq;

namespace FrequentDataMining.Clustering.OPTICS
{
    internal class PriorityQueue<T>
    {
        private List<T> items = new List<T>();

        public void Add(T item)
        {
            items.Add(item);
        }

        public void Remove(T item)
        {
            items.Remove(item);
        }

        public bool IsEmpty()
        {
            return !items.Any();
        }

        public T Poll()
        {
            var result = items.First();
            items = items.Skip(1).ToList();
            return result;
        }
    }
}
