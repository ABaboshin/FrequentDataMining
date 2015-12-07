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
        public double MinConfidence { get; set; }

        public double MinLift { get; set; }

        public Func<IEnumerable<Itemset<T>>> GetItemsets { get; set; }

        public Action<Rule<T>> SaveRule { get; set; }

        public int TransactionsCount { get; set; }

        public void Run()
        {
            TypeRegister.Register<int>((a, b) => { return a.CompareTo(b); });
            TypeRegister.EnsureType<T>();

            allFrequentItems = new List<Itemset<int>>();
            items = new List<T>();
            foreach (var itemset in GetItemsets())
            {
                allFrequentItems.Add(new Itemset<int>
                {
                    Support = itemset.Support,
                    Value = itemset.Value.Select(GetItemIdx).ToList()
                });
            }

            GetStrongRules(GenerateRules());
        }

        List<Itemset<int>> allFrequentItems;
        List<T> items;

        int GetItemIdx(T item)
        {
            for (var i = 0; i < items.Count(); i++)
            {
                if (TypeRegister.GetComparer<T>()(items[i], item) == 0)
                {
                    return i;
                }
            }

            items.Add(item);

            return items.Count() - 1;
        }

        HashSet<Rule<int>> GenerateRules()
        {
            var rulesList = new HashSet<Rule<int>>();

            for (var idx = 0; idx < allFrequentItems.Count(); idx++)
            {
                var item = allFrequentItems.Skip(idx).First();

                if (item.Value.Count() > 1)
                {
                    var subsetsList = GenerateSubsets(item.Value);

                    foreach (var subset in subsetsList)
                    {
                        var remaining = GetRemaining(subset, item.Value);
                        var rule = new Rule<int>(subset, remaining, 0, 0);

                        if (rulesList.All(r => !r.Equals(rule)))
                        {
                            rulesList.Add(rule);
                        }
                    }
                }
            }

            return rulesList;
        }

        List<List<int>> GenerateSubsets(IEnumerable<int> item)
        {
            var allSubsets = new List<List<int>>();
            var subsetLength = item.Count() / 2;

            for (var i = 1; i <= subsetLength; i++)
            {
                var subsets = new List<List<int>>();
                GenerateSubsetsRecursive(item, i, new int[item.Count()], subsets);
                allSubsets = allSubsets.Concat(subsets).ToList();
            }

            return allSubsets;
        }

        private void GenerateSubsetsRecursive(IEnumerable<int> item, int subsetLength, int[] temp, IList<List<int>> subsets, int q = 0, int r = 0)
        {
            if (q == subsetLength)
            {
                subsets.Add(temp.Take(subsetLength).ToList());
            }

            else
            {
                for (var i = r; i < item.Count(); i++)
                {
                    temp[q] = item.Skip(i).First();
                    GenerateSubsetsRecursive(item, subsetLength, temp, subsets, q + 1, i + 1);
                }
            }
        }

        IEnumerable<int> GetRemaining(IEnumerable<int> child, IEnumerable<int> parent)
        {
            return parent.Where(p => !child.Contains(p)).ToList();
        }

        void GetStrongRules(HashSet<Rule<int>> rules)
        {
            foreach (var rule in rules)
            {
                var xy = rule.Combination.Concat(rule.Remaining).ToList();
                xy.Sort();
                
                AddStrongRule(rule, xy);
            }
        }

        void AddStrongRule(Rule<int> rule, List<int> XY)
        {
            var confidence = GetConfidence(rule.Combination, XY);
            var lift = GetLift(rule.Combination, rule.Remaining, XY);

            if (confidence >= MinConfidence && lift >= MinLift)
            {
                var newRule = new Rule<int>(rule.Combination, rule.Remaining, confidence, lift);
                SaveRuleCaller(newRule);
            }

            confidence = GetConfidence(rule.Remaining, XY);
            lift = GetLift(rule.Remaining, rule.Combination, XY);

            if (confidence >= MinConfidence && lift >= MinLift)
            {
                var newRule = new Rule<int>(rule.Remaining, rule.Combination, confidence, lift);
                SaveRuleCaller(newRule);
            }
        }

        private void SaveRuleCaller(Rule<int> rule)
        {
            SaveRule(new Rule<T>
            {
                Lift = rule.Lift,
                Confidence = rule.Confidence,
                Combination = rule.Combination.Select(i=>items[i]).ToList(),
                Remaining = rule.Remaining.Select(i => items[i]).ToList()
            });
        }

        private Itemset<int> GetItemset(IEnumerable<int> item)
        {
            return allFrequentItems.FirstOrDefault(i => i.Value.Equal(item));
        }

        double GetConfidence(IEnumerable<int> X, IEnumerable<int> XY)
        {
            var x1 = GetItemset(XY);
            var x2 = GetItemset(X);
            return x1.Support / (double)x2.Support;
        }

        double GetLift(IEnumerable<int> X, IEnumerable<int> Y, IEnumerable<int> XY)
        {
            var term1 = ((double)GetItemset(XY).Support) / TransactionsCount;
            var term2 = (double)GetItemset(Y).Support / TransactionsCount;
            var term3 = ((double)GetItemset(X).Support / TransactionsCount);
            return term1 / (term2 * term3);
        }
    }
}
