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
        public Output<T> ProcessTransaction(double minSupport, double minConfidence, List<T> items, List<List<T>> transactions) {
            var frequentItems = GetL1FrequentItems(minSupport, items, transactions);
            var allFrequentItems = new FrequentItemsCollection<T>();
            allFrequentItems.ConcatItems(frequentItems);
            var candidates = new Dictionary<List<T>, double>();
            var transactionsCount = transactions.Count();

            do
            {
                candidates = GenerateCandidates(frequentItems, transactions);
                frequentItems = GetFrequentItems(candidates, minSupport, transactionsCount);
                allFrequentItems.ConcatItems(frequentItems);
            }
            while (candidates.Count != 0);

            var rules = GenerateRules(allFrequentItems);
            var strongRules = GetStrongRules(minConfidence, rules, allFrequentItems);

            return new Output<T>
            {
                StrongRules = strongRules,
                FrequentItems = allFrequentItems
            };
        }

        List<Rule<T>> GetStrongRules(double minConfidence, HashSet<Rule<T>> rules, FrequentItemsCollection<T> allFrequentItems)
        {
            var strongRules = new List<Rule<T>>();

            foreach (var rule in rules)
            {
                var xy = new Sorter<T>().Sort(rule.Combination.Concat(rule.Remaining).ToList());
                AddStrongRule(rule, xy, strongRules, minConfidence, allFrequentItems);
            }

            strongRules.Sort();
            return strongRules;
        }

        void AddStrongRule(Rule<T> rule, List<T> XY, List<Rule<T>> strongRules, double minConfidence, FrequentItemsCollection<T> allFrequentItems)
        {
            double confidence = GetConfidence(rule.Combination, XY, allFrequentItems);

            if (confidence >= minConfidence)
            {
                var newRule = new Rule<T>(rule.Combination, rule.Remaining, confidence);
                strongRules.Add(newRule);
            }

            confidence = GetConfidence(rule.Remaining, XY, allFrequentItems);

            if (confidence >= minConfidence)
            {
                var newRule = new Rule<T>(rule.Remaining, rule.Combination, confidence);
                strongRules.Add(newRule);
            }
        }

        double GetConfidence(List<T> X, List<T> XY, FrequentItemsCollection<T> allFrequentItems)
        {
            return (double)allFrequentItems[XY].Support / (double)allFrequentItems[X].Support;
        }

        HashSet<Rule<T>> GenerateRules(FrequentItemsCollection<T> allFrequentItems)
        {
            var rulesList = new HashSet<Rule<T>>();

            for (int idx = 0; idx < allFrequentItems.Count(); idx++)
            {
                var item = allFrequentItems[idx];

                if (item.Value.Count() > 1)
                {
                    var subsetsList = GenerateSubsets(item.Value);

                    foreach (var subset in subsetsList)
                    {
                        var remaining = GetRemaining(subset, item.Value);
                        var rule = new Rule<T>(subset, remaining, 0);

                        if (rulesList.All(r=>!r.Equals(rule)))
                        {
                            //Console.WriteLine(rule);
                            rulesList.Add(rule);
                        }
                    }
                }
            }

            return rulesList;
        }

        List<T> GetRemaining(List<T> child, List<T> parent)
        {
            var copy = parent.Select(p => p).ToList();
            for (int i = 0; i < child.Count(); i++)
            {
                int index = copy.IndexOf(child[i]);
                copy.RemoveAt(index);
            }

            return copy;
        }

        List<List<T>> GenerateSubsets(List<T> item)
        {
            var allSubsets = new List<List<T>>();
            int subsetLength = item.Count() / 2;

            for (int i = 1; i <= subsetLength; i++)
            {
                var subsets = new List<List<T>>();
                GenerateSubsetsRecursive(item, i, new T[item.Count], subsets);
                allSubsets = allSubsets.Concat(subsets).ToList();
            }

            return allSubsets;
        }

        private void GenerateSubsetsRecursive(List<T> item, int subsetLength, T[] temp, IList<List<T>> subsets, int q = 0, int r = 0)
        {
            if (q == subsetLength)
            {
                subsets.Add(temp.Take(subsetLength).ToList());
            }

            else
            {
                for (int i = r; i < item.Count(); i++)
                {
                    temp[q] = item[i];
                    GenerateSubsetsRecursive(item, subsetLength, temp, subsets, q + 1, i + 1);
                }
            }
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
