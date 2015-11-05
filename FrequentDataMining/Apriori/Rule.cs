﻿// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;

namespace FrequentDataMining.Apriori
{
    public class Rule<T> : IComparable<Rule<T>> where T : IComparable<T>, IEquatable<T>
    {
        public List<T> Combination { get; private set; }
        public List<T> Remaining { get; private set; }
        public double Confidence { get; private set; }

        public Rule(List<T> combination, List<T> remaining, double confidence)
        {
            Combination = combination;
            Remaining = remaining;
            Confidence = confidence;
        }

        public int CompareTo(Rule<T> other)
        {
            return Combination.Compare(other.Combination);
        }

        public override int GetHashCode()
        {
            var sorted = new Sorter<T>().Sort(Combination.Concat(Remaining).ToList());
            return sorted.GetHashCode();
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
