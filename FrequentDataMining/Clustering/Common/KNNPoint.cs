// MIT License.
// (c) 2015, Andrey Baboshin

using System;

namespace FrequentDataMining.Clustering.Common
{
    internal class KNNPoint : IComparable<KNNPoint>, IEquatable<KNNPoint>
    {
        public VectorData Data { get; set; }
        public double Distance { get; set; }

        public KNNPoint(VectorData data, double distance)
        {
            Data = data;
            Distance = distance;
        }

        public int CompareTo(KNNPoint other)
        {
            return Distance.CompareTo(other.Distance);
        }

        public bool Equals(KNNPoint other)
        {
            return Distance.Equals(other.Distance);
        }
    }
}
