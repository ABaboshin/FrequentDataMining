// MIT License.
// (c) 2015, Andrey Baboshin

using System.Collections.Generic;
using FrequentDataMining.Clustering.Common;

namespace FrequentDataMining.Clustering.OPTICS
{
    internal class VectorDataOptics : VectorData
    {
        public double ReachabilityDistance { get; set; }

        public double CoreDistance { get; set; }

        public VectorDataOptics()
        {
            ReachabilityDistance = double.PositiveInfinity;
            CoreDistance = double.PositiveInfinity;
        }

        public void SetCoreDistance(List<KNNPoint> neighboors, double epsilon,
            int minPts)
        {
            if (neighboors.Count < minPts - 1)
            {
                CoreDistance = double.PositiveInfinity;
            }
            else
            {
                neighboors.Sort();
                CoreDistance = neighboors[minPts - 2].Distance;
            }
        }
    }
}
