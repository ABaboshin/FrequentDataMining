// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;
using FrequentDataMining.Common;

namespace FrequentDataMining.AgrawalFaster
{
    /// <summary>
    /// Agrawal fast mining association rules
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AgrawalFaster<T>
    {
        public List<Rule<T>> Run(double minConfidence, double minLift, IEnumerable<Itemset<T>> allFrequentItems, int transactionsCount)
        {
            if (TypeRegister.GetSorter<T>() == null || TypeRegister.GetComparer<T>() == null)
            {
                throw new NotImplementedException("You need register the type at first by calling the FrequentDataMining.Common.Register()");
            }

            return GetStrongRules(minConfidence, minLift, GenerateRules(allFrequentItems), allFrequentItems, transactionsCount);
        }

        HashSet<Rule<T>> GenerateRules(IEnumerable<Itemset<T>> allFrequentItems)
        {
            var rulesList = new HashSet<Rule<T>>();

            for (var idx = 0; idx < allFrequentItems.Count(); idx++)
            {
                var item = allFrequentItems.Skip(idx).First();

                if (item.Value.Count() > 1)
                {
                    var subsetsList = GenerateSubsets(item.Value);

                    foreach (var subset in subsetsList)
                    {
                        var remaining = GetRemaining(subset, item.Value);
                        var rule = new Rule<T>(subset, remaining, 0, 0);

                        if (rulesList.All(r => !r.Equals(rule)))
                        {
                            rulesList.Add(rule);
                        }
                    }
                }
            }

            return rulesList;
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

        IEnumerable<T> GetRemaining(IEnumerable<T> child, IEnumerable<T> parent)
        {
            return parent.Where(p => !child.Contains(p)).ToList();
        }

        List<Rule<T>> GetStrongRules(double minConfidence, double minLift, HashSet<Rule<T>> rules, IEnumerable<Itemset<T>> allFrequentItems, int size)
        {
            var strongRules = new List<Rule<T>>();

            foreach (var rule in rules)
            {
                var sorter = TypeRegister.GetSorter<T>();
                var xy = sorter(rule.Combination.Concat(rule.Remaining).ToList()).ToList();
                
                AddStrongRule(rule, xy, strongRules, minConfidence, minLift, allFrequentItems, size);
            }

            strongRules.Sort();
            return strongRules;
        }

        void AddStrongRule(Rule<T> rule, List<T> XY, List<Rule<T>> strongRules, double minConfidence, double minLift, IEnumerable<Itemset<T>> allFrequentItems, int size)
        {
            var confidence = GetConfidence(rule.Combination, XY, allFrequentItems);
            var lift = GetLift(rule.Combination, rule.Remaining, XY, allFrequentItems, size);

            if (confidence >= minConfidence && lift >= minLift)
            {
                var newRule = new Rule<T>(rule.Combination, rule.Remaining, confidence, lift);
                strongRules.Add(newRule);
            }

            confidence = GetConfidence(rule.Remaining, XY, allFrequentItems);
            lift = GetLift(rule.Remaining, rule.Combination, XY, allFrequentItems, size);

            if (confidence >= minConfidence && lift >= minLift)
            {
                var newRule = new Rule<T>(rule.Remaining, rule.Combination, confidence, lift);
                strongRules.Add(newRule);
            }
        }

        private Itemset<T> GetItemset(IEnumerable<Itemset<T>> allFrequentItems, IEnumerable<T> item)
        {
            return allFrequentItems.FirstOrDefault(i => i.Value.Equal(item));
        }

        double GetConfidence(IEnumerable<T> X, IEnumerable<T> XY, IEnumerable<Itemset<T>> allFrequentItems)
        {
            var x1 = GetItemset(allFrequentItems, XY);
            var x2 = GetItemset(allFrequentItems, X);
            return x1.Support / (double)x2.Support;
        }

        private double GetLift(IEnumerable<T> X, IEnumerable<T> Y, IEnumerable<T> XY, IEnumerable<Itemset<T>> allFrequentItems, int size)
        {
            var term1 = ((double)GetItemset(allFrequentItems, XY).Support) / size;
            var term2 = (double)GetItemset(allFrequentItems, Y).Support / size;
            var term3 = ((double)GetItemset(allFrequentItems, X).Support / size);
            return term1 / (term2 * term3);
        }
    }
}
