// MIT License.
// (c) 2015, Andrey Baboshin

using System.Collections.Generic;

namespace FrequentDataMining.Markov
{
    class MarkovState
    {
        public int Count { get; private set; }

        Dictionary<int, int> transitions = new Dictionary<int, int>();

        public MarkovState()
        {
            Count = 0;
        }

        public void AddTransition(int val)
        {
            int support;
            if (!transitions.TryGetValue(val, out support))
            {
                transitions.Add(val, 0);
                support = 0;
                Count += 1;
            }

            support += 1;
            transitions[val] = support;
        }

        public int? GetBestNextState()
        {
            var highestCount = 0;
            var highestValue = 0;
            bool found = false;

            foreach (var transition in transitions)
            {
                if (transition.Value > highestCount)
                {
                    highestCount = transition.Value;
                    highestValue = transition.Key;
                    found = true;
                }
            }

            return found ? (int?) highestValue : null;
        }
    }
}
