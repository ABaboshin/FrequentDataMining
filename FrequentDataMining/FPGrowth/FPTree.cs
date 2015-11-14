// MIT License.
// (c) 2015, Andrey Baboshin

using System.Collections.Generic;
using System.Linq;

namespace FrequentDataMining.FPGrowth
{
    internal class FPTree
    {
        /// <summary>
        /// List of items in the header table
        /// </summary>
        public List<int> HeaderList { get; set; }

        /// <summary>
        /// List of pairs (item, frequency) of the header table
        /// </summary>
        public Dictionary<int, FPNode> MapItemNodes { get; set; }

        /// <summary>
        /// Map that indicates the last node for each item using the node links
        /// </summary>
        Dictionary<int, FPNode> mapItemLastNode = new Dictionary<int, FPNode>();

        public FPNode Root { get; set; }

        public FPTree()
        {
            Root = new FPNode();
            MapItemNodes = new Dictionary<int, FPNode>();
        }

        /// <summary>
        /// add a transaction to the fp-tree (for the initial construction of the FP-Tree).
        /// </summary>
        /// <param name="transaction"></param>
        public void AddTransaction(IEnumerable<int> transaction)
        {
            var currentNode = Root;
            foreach (var item in transaction)
            {
                var child = currentNode.GetChild(item);
                if (child == null)
                {
                    var newNode = new FPNode
                    {
                        Item = item,
                        Parent = currentNode,
                        Counter = 1
                    };
                    currentNode.Childs.Add(newNode);

                    currentNode = newNode;

                    FixNodeLinks(item, newNode);
                }
                else
                {
                    child.Counter++;
                    currentNode = child;
                }
            }
        }

        /// <summary>
        /// create the list of items in the header table in descending order of support
        /// </summary>
        /// <param name="mapSupport"></param>
        public void CreateHeaderList(Dictionary<int, int> mapSupport)
        {
            HeaderList = mapSupport.OrderByDescending(s => s.Value).Select(s => s.Key).ToList();
        }

        public void AddPrefixPath(List<FPNode> prefixPath, Dictionary<int, int> mapSupportBeta, int relativeMinsupp)
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
                        var newNode = new FPNode();
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
        void FixNodeLinks(int item, FPNode newNode)
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
