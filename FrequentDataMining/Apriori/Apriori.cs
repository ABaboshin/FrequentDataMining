// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;
using FrequentDataMining.Common;

namespace FrequentDataMining.Apriori
{
    public class Apriori<T> where T : IComparable<T>, IEquatable<T>
    {
        public List<Itemset<T>> ProcessTransaction(double minSupport, /*List<T> items,*/ List<List<T>> transactions)
        {
            var items = transactions.SelectMany(t => t).Distinct();
            var frequentItems = GetL1FrequentItems(minSupport, items, transactions);
            var allFrequentItems = new List<Itemset<T>>();
            allFrequentItems.AddRange(frequentItems);
            var candidates = new Dictionary<List<T>, double>();
            var transactionsCount = transactions.Count();

            do
            {
                candidates = GenerateCandidates(frequentItems, transactions);
                frequentItems = GetFrequentItems(candidates, minSupport, transactionsCount);
                allFrequentItems.AddRange(frequentItems);
            }
            while (candidates.Count != 0);

            return allFrequentItems;
        }

        
        List<Itemset<T>> GetFrequentItems(IDictionary<List<T>, double> candidates, double minSupport, double transactionsCount)
        {
            var frequentItems = new List<Itemset<T>>();

            foreach (var item in candidates)
            {
                if (item.Value / transactionsCount >= minSupport)
                {
                    frequentItems.Add(new Itemset<T> { Value = item.Key, Support = (int)item.Value });
                }
            }

            return frequentItems;
        }

        private Dictionary<List<T>, double> GenerateCandidates(IList<Itemset<T>> frequentItems, IEnumerable<List<T>> transactions)
        {
            var candidates = new Dictionary<List<T>, double>();

            for (int i = 0; i < frequentItems.Count - 1; i++)
            {
                var firstItem = new Sorter<T>().Sort(frequentItems[i].Value);

                for (int j = i + 1; j < frequentItems.Count; j++)
                {
                    var secondItem = new Sorter<T>().Sort(frequentItems[j].Value);
                    var generatedCandidate = GenerateCandidate(firstItem, secondItem);

                    if (generatedCandidate.Any())
                    {
                        var support = GetSupport(generatedCandidate, transactions);
                        candidates.Add(generatedCandidate, support);
                    }
                }
            }

            return candidates;
        }

        List<T> GenerateCandidate(List<T> firstItem, List<T> secondItem)
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
                    return firstItem.Concat(new List<T> { secondItem.Last() }).ToList();
                }

                return new List<T>();
            }
        }

        List<Itemset<T>> GetL1FrequentItems(double minSupport, IEnumerable<T> items, IEnumerable<List<T>> transactions)
        {
            var frequentItemsL1 = new List<Itemset<T>>();
            var transactionsCount = transactions.Count();

            foreach (var item in items)
            {
                var support = GetSupport(new List<T> { item }, transactions);

                if (support / transactionsCount >= minSupport)
                {
                    frequentItemsL1.Add(new Itemset<T> { Value = new List<T> { item }, Support = (int)support });
                }
            }
            frequentItemsL1.Sort();
            return frequentItemsL1;
        }

        double GetSupport(List<T> generatedCandidate, IEnumerable<List<T>> transactionsList)
        {
            double support = 0;

            foreach (var transaction in transactionsList)
            {
                if (CheckIsSubset(generatedCandidate, transaction))
                {
                    support++;
                }
            }

            return support;
        }

        bool CheckIsSubset(List<T> child, List<T> parent)
        {
            foreach (var c in child)
            {
                if (!parent.Contains(c))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
