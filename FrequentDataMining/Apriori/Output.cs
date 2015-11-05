// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;

namespace FrequentDataMining.Apriori
{
    public class Output<T> where T : IComparable<T>, IEquatable<T>
    {
        public List<Rule<T>> StrongRules { get; set; }

        public FrequentItemsCollection<T> FrequentItems { get; set; }
    }
}
