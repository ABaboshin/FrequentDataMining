// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;
using FrequentDataMining.Common;

namespace FrequentDataMining.Markov
{
    public class MarkovPredictor<T>
    {
        /// <summary>
        /// Order of the model
        /// </summary>
        public int K { get; set; }

        public MarkovPredictor()
        {
            TypeRegister.Register<int>((a, b) => { return a.CompareTo(b); });
        }

        public bool Train(Func<IEnumerable<IEnumerable<T>>> getTransactions)
        {
            foreach (var seq in getTransactions())
            {
                var mappedItems = seq.Select(GetItemIdx).ToList();
                
                for (var i = 0; i < mappedItems.Count() - 1; i++)
                {
                    var k = mappedItems.Count() - i > K ? K : mappedItems.Count() - i - 1;
                    for (var c = 0; c <= k; c++)
                    {
                        var key = new List<int>();
                        for (var j = 0; j < c; j++)
                        {
                            key.Add(mappedItems[i+j]);
                        }

                        var idx = GetKeyIndex(key);
                        if (idx > states.Count() - 1)
                        {
                            states.Add(new MarkovState());
                        }

                        states[idx].AddTransition(mappedItems[i + c]);
                    }
                }
            }

            return true;
        }

        public IEnumerable<T> Predict(IEnumerable<T> target)
        {
            var mappedItems = target.Select(GetItemIdx).ToList();
            var k = mappedItems.Count() > K ? K : mappedItems.Count() - 1;

            for (var i = k; i > 0; i--)
            {
                var key = new List<int>();
                for (var j = mappedItems.Count() - i; j < mappedItems.Count(); j++)
                {
                    key.Add(mappedItems[j]);
                }

                var idx = GetKeyIndex(key);
                MarkovState state = null;
                if (idx < states.Count() - 1)
                {
                    state = states[idx];
                }

                var nextState = state?.GetBestNextState();
                if (nextState != null)
                {
                    var predict = new List<T> { items[nextState.Value] };
                    return predict;
                }
            }

            return new List<T>();
        }

        List<List<int>> keys = new List<List<int>>();
        List<MarkovState> states = new List<MarkovState>();

        private int GetKeyIndex(List<int> key)
        {
            for (var i = 0; i < keys.Count(); i++)
            {
                if (keys[i].Equal(key))
                {
                    return i;
                }
            }

            keys.Add(key);

            return keys.Count() - 1;
        }


        List<T> items = new List<T>();

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
