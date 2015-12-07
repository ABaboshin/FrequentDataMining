// MIT License.
// (c) 2015, Andrey Baboshin

using System.Collections.Generic;
using System.Linq;
using FrequentDataMining.Common;

namespace FrequentDataMining.FPGrowth
{
    public class FPGrowth<T> : AssociationRuleLearningAlgorithm<T>
    {
        public override void ProcessTransactions()
        {
            Prepare();

            var mapSupport = ScanDetermineFrequencyOfSingleItems();
            var transactionsCount = transactions.Count();
            minSupportRelative = (int) (MinSupport*transactionsCount);
            var tree = new FPTree ();

            foreach (var transaction in transactions)
            {
                tree.AddTransaction(
                    transaction
                    .Where(i=>mapSupport[i] >= MinSupport)
                    .OrderByDescending(i=>mapSupport[i])
                );
            }

            tree.CreateHeaderList(mapSupport);

            nodeBuffer = ListExtensions.Allocate<FPNode>(BufferSize);

            Run(tree, ListExtensions.Allocate<int>(BufferSize), 0, transactionsCount, mapSupport);
        }

        #region fields
        /// <summary>
        /// the relative minimum support
        /// </summary>
        int minSupportRelative;

        List<FPNode> nodeBuffer;
        #endregion

        public const int BufferSize = 200;

        void Run(FPTree tree, List<int> prefix, int prefixLength, int prefixSupport,
            Dictionary<int, int> mapSupport)
        {
            var singlePath = true;
            var singlePathSupport = 0;
            var position = 0;

            if (tree.Root.Childs.Count() > 1)
            {
                singlePath = false;
            }
            else
            {
                var currentNode = tree.Root.Childs[0];
                while (true)
                {
                    if (currentNode.Childs.Count() > 1)
                    {
                        singlePath = false;
                        break;
                    }

                    nodeBuffer[position] = currentNode;
                    position++;

                    if (!currentNode.Childs.Any())
                    {
                        break;
                    }

                    currentNode = currentNode.Childs[0];
                }
            }

            if (singlePath && singlePathSupport >= minSupportRelative)
            {
                SaveAllCombinations(position, prefix, prefixLength);
            }
            else
            {
                for (var i = tree.HeaderList.Count() - 1; i >= 0; i--)
                {
                    var item = tree.HeaderList[i];
                    var support = mapSupport[item];

                    prefix[prefixLength] = item;

                    var betaSupport = (prefixSupport < support) ? prefixSupport : support;
                    SaveItemsetCaller(prefix, prefixLength + 1, betaSupport);

                    var prefixPaths = new List<List<FPNode>>();
                    FPNode path = null;

                    tree.MapItemNodes.TryGetValue(item, out path);

                    var mapSupportBeta = new Dictionary<int, int>();
                    while (path != null)
                    {
                        if (path.Parent.Item != -1)
                        {
                            var prefixPath = new List<FPNode>();
                            prefixPath.Add(path);

                            var pathCount = path.Counter;

                            var parent = path.Parent;
                            while (parent.Item != -1)
                            {
                                prefixPath.Add(parent);

                                if (!mapSupportBeta.ContainsKey(parent.Item))
                                {
                                    mapSupportBeta.Add(parent.Item, 0);
                                }

                                mapSupportBeta[parent.Item] += pathCount;

                                parent = parent.Parent;
                            }

                            prefixPaths.Add(prefixPath);
                        }

                        path = path.NodeLink;
                    }

                    var treeBeta = new FPTree();
                    foreach (var prefixPath in prefixPaths)
                    {
                        treeBeta.AddPrefixPath(prefixPath, mapSupportBeta, minSupportRelative);
                    }

                    if (treeBeta.Root.Childs.Any())
                    {
                        treeBeta.CreateHeaderList(mapSupportBeta);

                        Run(treeBeta, prefix, prefixLength + 1, betaSupport, mapSupportBeta);
                    }
                }
            }
        }

        private void SaveAllCombinations(int position, List<int> prefix, int prefixLength)
        {
            var support = 0;
            for (long i = 0; i < (1 << position); i++)
            {
                var newPrefixLength = prefixLength;

                for (int j = 0; j < position; j++)
                {
                    var isSet = (int) i & (1 << j);
                    if (isSet > 0)
                    {
                        prefix[newPrefixLength++] = nodeBuffer[j].Item;
                        if (support == 0)
                        {
                            support = nodeBuffer[j].Counter;
                        }
                    }
                }

                SaveItemsetCaller(prefix, newPrefixLength, support);
            }
        }

        void SaveItemsetCaller(IEnumerable<int> prefix, int itemsetLength, int support)
        {
            SaveItemset(new Itemset<T>
            {
                Support = support,
                Value = prefix.Take(itemsetLength).Select(i => items[i]).ToList()
            });
        }

        /// <summary>
        /// scan the input to calculate the support of single items
        /// </summary>
        /// <returns></returns>
        Dictionary<int, int> ScanDetermineFrequencyOfSingleItems()
        {
            var result = new Dictionary<int, int>();

            foreach (var transaction in transactions)
            {
                foreach (var item in transaction)
                {
                    if (!result.ContainsKey(item))
                    {
                        result.Add(item, 0);
                    }

                    result[item] += 1;
                }
            }

            return result;
        }
    }
}
