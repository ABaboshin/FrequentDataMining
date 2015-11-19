using System.Collections.Generic;
using FrequentDataMining.DBScan;

namespace FrequentDataMining.Common.DistanceFunctions
{
    internal abstract class DistanceFunction
    {
        public abstract double CalculateDistance(VectorData a, VectorData b);
    }
}
