// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;
using FrequentDataMining.Common;

namespace FrequentDataMining.AgrawalFaster
{
    public class Rule<T> : IComparable<Rule<T>>
    {
        public IEnumerable<T> Combination { get; private set; }

        public IEnumerable<T> Remaining { get; private set; }

        public double Confidence { get; private set; }

        public double Lift { get; private set; }

        public Rule(IEnumerable<T> combination, IEnumerable<T> remaining, double confidence, double lift)
        {
            Combination = combination;
            Remaining = remaining;
            Confidence = confidence;
            Lift = lift;
        }

        public int CompareTo(Rule<T> other)
        {
            return Combination.Compare(other.Combination);
        }

        public override int GetHashCode()
        {
            var sorter = TypeRegister.GetSorter<T>();
            return sorter(Combination.Concat(Remaining)).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Rule<T>;
            if (other == null)
            {
                return false;
            }

            return Combination.Equal(other.Combination) && Remaining.Equal(other.Remaining) || Combination.Equal(other.Remaining) && Remaining.Equal(other.Combination);
        }

        public override string ToString()
        {
            return string.Join("", Combination.Select(c=>c.ToString())) + ", " + string.Join("", Remaining.Select(c=>c.ToString())) + ": " + Confidence;
        }
    }
}
