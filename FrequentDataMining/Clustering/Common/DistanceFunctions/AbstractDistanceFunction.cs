// MIT License.
// (c) 2015, Andrey Baboshin

namespace FrequentDataMining.Clustering.Common.DistanceFunctions
{
    public abstract class AbstractDistanceFunction
    {
        public abstract double CalculateDistance(VectorData a, VectorData b);
    }
}
