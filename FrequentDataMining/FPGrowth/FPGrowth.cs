// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;
using FrequentDataMining.Common;

namespace FrequentDataMining.FPGrowth
{
    public class FPGrowth<T> where T : class, IComparable<T>, IEquatable<T>
    {
        public List<Itemset<T>> ProcessTransactions(double minSupport, List<List<T>> transactions)
        {
            Result = new List<Itemset<T>>();

            var mapSupport = ScanDetermineFrequencyOfSingleItems(transactions);
            minSupportRelative = (int) (minSupport*transactions.Count());
            var tree = new FPTree<T>();

            foreach (var transaction in transactions)
            {
                tree.AddTransaction(
                    transaction
                    .Where(i=>mapSupport[i] >= minSupport)
                    .OrderByDescending(i=>mapSupport[i])
                    .ToList()
                );
            }

            tree.CreateHeaderList(mapSupport);

            nodeBuffer = ListExtensions.Allocate<FPNode<T>>(BufferSize);

            Run(tree, ListExtensions.Allocate<T>(BufferSize), 0, transactions.Count(), mapSupport);

            return Result;
        }

        /// <summary>
        /// Result
        /// </summary>
        public List<Itemset<T>> Result { get; set; }

        /// <summary>
        /// the relative minimum support
        /// </summary>
        int minSupportRelative;

        List<FPNode<T>> nodeBuffer;

        public const int BufferSize = 200;

        void Run(FPTree<T> tree, List<T> prefix, int prefixLength, int prefixSupport,
            Dictionary<T, int> mapSupport)
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
                    SaveItemset(prefix, prefixLength + 1, betaSupport);

                    var prefixPaths = new List<List<FPNode<T>>>();
                    var path = tree.MapItemNodes[item];

                    var mapSupportBeta = new Dictionary<T, int>();
                    while (path != null)
                    {
                        if (path.Parent.Item != null)
                        {
                            var prefixPath = new List<FPNode<T>>();
                            prefixPath.Add(path);

                            var pathCount = path.Counter;

                            var parent = path.Parent;
                            while (parent.Item != null)
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

                    var treeBeta = new FPTree<T>();
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

        private void SaveAllCombinations(int position, List<T> prefix, int prefixLength)
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

                SaveItemset(prefix, newPrefixLength, support);
            }
        }

        private void SaveItemset(List<T> prefix, int itemsetLength, int support)
        {
            //Console.WriteLine(string.Join(";", prefix.Take(itemsetLength)) + " SUP# " + support);
            Result.Add(new Itemset<T>
            {
                Support = support,
                Value = prefix.Take(itemsetLength).ToList()
            });
        }

        /// <summary>
        /// scan the input to calculate the support of single items
        /// </summary>
        /// <param name="transactions"></param>
        /// <returns></returns>
        private Dictionary<T, int> ScanDetermineFrequencyOfSingleItems(List<List<T>> transactions)
        {
            var result = new Dictionary<T, int>();

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
