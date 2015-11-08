// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;

namespace FrequentDataMining.FPGrowth
{
    internal class FPNode<T> where T : IComparable<T>, IEquatable<T>
    {
        public T Item { get; set; }

        /// <summary>
        /// frequency counter  (a.k.a. support)
        /// </summary>
        public int Counter { get; set; }

        /// <summary>
        /// the parent node of that node or null if it is the root
        /// </summary>
        public FPNode<T> Parent { get; set; }

        /// <summary>
        /// the child nodes of that node
        /// </summary>
        public List<FPNode<T>> Childs { get; set; }

        /// <summary>
        /// link to next node with the same Item id (for the header table)
        /// </summary>
        public FPNode<T> NodeLink { get; set; }

        public FPNode()
        {
            Childs = new List<FPNode<T>>();
        }

        /// <summary>
        /// Return the immediate child of this node having a given ID.
        /// If there is no such child, return null;
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        internal FPNode<T> GetChild(T i)
        {
            return Childs.FirstOrDefault(c => c.Item.Equals(i));
        }

        public override string ToString()
        {
            return Item.ToString();
        }
    }
}
