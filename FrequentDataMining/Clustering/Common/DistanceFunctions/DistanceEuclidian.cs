// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Linq;

namespace FrequentDataMining.Clustering.Common.DistanceFunctions
{
    public class DistanceEuclidian : AbstractDistanceFunction
    {
        public override double CalculateDistance(VectorData a, VectorData b)
        {
            double sum = 0;
            for (var i = 0; i < a.Data.Count(); i++)
            {
                sum += Math.Pow((a.Data.Skip(i).First() - b.Data.Skip(i).First()), 2);
            }

            return Math.Sqrt(sum);
        }
    }
}
