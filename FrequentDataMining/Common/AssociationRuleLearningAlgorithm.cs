// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;

namespace FrequentDataMining.Common
{
    public abstract class AssociationRuleLearningAlgorithm<T>
    {
        public double MinSupport { get; set; }

        public Func<IEnumerable<IEnumerable<T>>> GetTransactions { get; set; }

        public Action<Itemset<T>> SaveItemset { get; set; }

        public abstract void ProcessTransactions();

        protected void Prepare()
        {
            TypeRegister.Register<int>((a, b) => { return a.CompareTo(b); });
            TypeRegister.EnsureType<T>();

            transactions = new List<List<int>>();
            items = new List<T>();
            foreach (var transaction in GetTransactions())
            {
                transactions.Add(
                    transaction.Select(GetItemIdx).ToList()
                );
            }
        }

        protected List<List<int>> transactions;
        protected List<T> items;

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
    }
}
