// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;

namespace FrequentDataMining.FPGrowth
{
    internal class FPTree<T> where T : IComparable<T>, IEquatable<T>
    {
        /// <summary>
        /// List of items in the header table
        /// </summary>
        public List<T> HeaderList { get; set; }

        /// <summary>
        /// List of pairs (item, frequency) of the header table
        /// </summary>
        public Dictionary<T, FPNode<T>> MapItemNodes { get; set; }

        /// <summary>
        /// Map that indicates the last node for each item using the node links
        /// </summary>
        Dictionary<T, FPNode<T>> mapItemLastNode = new Dictionary<T, FPNode<T>>();

        public FPNode<T> Root { get; set; }

        public FPTree()
        {
            Root = new FPNode<T>();
            MapItemNodes = new Dictionary<T, FPNode<T>>();
        }

        /// <summary>
        /// add a transaction to the fp-tree (for the initial construction of the FP-Tree).
        /// </summary>
        /// <param name="transaction"></param>
        public void AddTransaction(List<T> transaction)
        {
            //Console.WriteLine("add " + string.Join(";", transaction));

            var currentNode = Root;
            foreach (var item in transaction)
            {
                var child = currentNode.GetChild(item);
                if (child == null)
                {
                    
                    var newNode = new FPNode<T>();
                    newNode.Item = item;
                    newNode.Parent = currentNode;
                    newNode.Counter = 1;
                    currentNode.Childs.Add(newNode);

                    currentNode = newNode;

                    //Console.WriteLine(" create node " + newNode);

                    FixNodeLinks(item, newNode);
                }
                else
                {
                    //Console.WriteLine(" update node " + child);
                    child.Counter++;
                    currentNode = child;
                }
            }
        }

        /// <summary>
        /// create the list of items in the header table in descending order of support
        /// </summary>
        /// <param name="mapSupport"></param>
        public void CreateHeaderList(Dictionary<T, int> mapSupport)
        {
            HeaderList = mapSupport.OrderByDescending(s => s.Value).Select(s => s.Key).ToList();
        }

        public void AddPrefixPath(List<FPNode<T>> prefixPath, Dictionary<T, int> mapSupportBeta, int relativeMinsupp)
        {
            var pathCount = prefixPath.First().Counter;

            var currentNode = Root;

            for (var i = prefixPath.Count() - 1; i >= 1; i--)
            {
                var pathItem = prefixPath[i];

                if (mapSupportBeta[pathItem.Item] >= relativeMinsupp)
                {
                    var child = currentNode.GetChild(pathItem.Item);
                    if (child == null)
                    {
                        var newNode = new FPNode<T>();
                        newNode.Item = pathItem.Item;
                        newNode.Parent = currentNode;
                        newNode.Counter = pathCount;
                        currentNode.Childs.Add(newNode);
                        currentNode = newNode;

                        FixNodeLinks(pathItem.Item, newNode);
                    }
                    else
                    {
                        child.Counter += pathCount;
                        currentNode = child;
                    }
                }
            }
        }

        /// <summary>
        /// fix the node link for an item after inserting a new node
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newNode"></param>
        void FixNodeLinks(T item, FPNode<T> newNode)
        {
            if (mapItemLastNode.ContainsKey(item))
            {
                mapItemLastNode[item].NodeLink = newNode;
                mapItemLastNode[item] = newNode;
            }
            else
            {
                mapItemLastNode.Add(item, newNode);
            }
            
            if (!MapItemNodes.ContainsKey(item))
            {
                MapItemNodes.Add(item, newNode);
            }
        }
    }
}
