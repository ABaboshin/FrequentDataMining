// MIT License.
// (c) 2015, Andrey Baboshin

using System.Collections.Generic;
using System.Linq;
using FrequentDataMining.Common;

namespace FrequentDataMining.Apriori
{
    public class Apriori<T> : AssociationRuleLearningAlgorithm<T>
    {
        public override void ProcessTransactions()
        {
            Prepare();

            var frequentItems = GetL1FrequentItems();
            //var allFrequentItems = new List<Itemset<int>>();

            foreach (var frequentItem in frequentItems)
            {
                SaveItemset(frequentItem);
            }

            //allFrequentItems.AddRange(frequentItems);
            var candidates = new Dictionary<List<int>, double>();

            do
            {
                candidates = GenerateCandidates(frequentItems);
                frequentItems = GetFrequentItems(candidates);
                foreach (var frequentItem in frequentItems)
                {
                    SaveItemset(frequentItem);
                }
                //allFrequentItems.AddRange(frequentItems);
            }
            while (candidates.Count != 0);

            //return allFrequentItems;
        }

        void SaveItemset(Itemset<int> itemset)
        {
            ItemsetWriter.SaveItemset(new Itemset<T>
            {
                Support = itemset.Support,
                Value = itemset.Value.Select(i => items[i]).ToList()
            });
        }

        List<Itemset<int>> GetFrequentItems(IDictionary<List<int>, double> candidates)
        {
            var frequentItems = new List<Itemset<int>>();

            foreach (var item in candidates)
            {
                if (item.Value / transactions.Count() >= MinSupport)
                {
                    frequentItems.Add(new Itemset<int> { Value = item.Key, Support = (int)item.Value });
                }
            }

            return frequentItems;
        }

        private Dictionary<List<int>, double> GenerateCandidates(IList<Itemset<int>> frequentItems)
        {
            var candidates = new Dictionary<List<int>, double>();

            for (var i = 0; i < frequentItems.Count - 1; i++)
            {
                var sorter = TypeRegister.GetSorter<T>();
                var firstItem = frequentItems[i].Value.ToList();
                firstItem.Sort();

                for (var j = i + 1; j < frequentItems.Count; j++)
                {
                    var secondItem = frequentItems[j].Value.ToList();
                    secondItem.Sort();
                    var generatedCandidate = GenerateCandidate(firstItem, secondItem);

                    if (generatedCandidate.Any())
                    {
                        var support = GetSupport(generatedCandidate);
                        candidates.Add(generatedCandidate, support);
                    }
                }
            }

            return candidates;
        }

        List<int> GenerateCandidate(List<int> firstItem, List<int> secondItem)
        {
            var length = firstItem.Count();

            if (firstItem.Count() == 1)
            {
                return firstItem.Concat(secondItem).ToList();
            }
            else
            {
                var firstSubString = firstItem.Take(length - 1).ToList();
                var secondSubString = secondItem.Take(length - 1).ToList();

                if (firstSubString.Equal(secondSubString))
                {
                    return firstItem.Concat(new List<int> { secondItem.Last() }).ToList();
                }

                return new List<int>();
            }
        }

        List<Itemset<int>> GetL1FrequentItems()
        {
            var frequentItemsL1 = new List<Itemset<int>>();
            var transactionsCount = transactions.Count();

            for (int item = 0; item < items.Count(); item++)
            {
                var support = GetSupport(new List<int> { item });

                if (support / transactionsCount >= MinSupport)
                {
                    frequentItemsL1.Add(new Itemset<int> { Value = new List<int> { item }, Support = (int)support });
                }
            }

            frequentItemsL1.Sort();
            return frequentItemsL1;
        }

        double GetSupport(List<int> generatedCandidate)
        {
            double support = 0;

            foreach (var transaction in transactions)
            {
                if (CheckIsSubset(generatedCandidate, transaction))
                {
                    support++;
                }
            }

            return support;
        }

        bool CheckIsSubset(IEnumerable<int> child, IEnumerable<int> parent)
        {
            return child.All(parent.Contains);
        }
    }
}
