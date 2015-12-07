// MIT License.
// (c) 2015, Andrey Baboshin

using System.Collections.Generic;
using FrequentDataMining.Clustering.Common.DistanceFunctions;
using FrequentDataMining.Clustering.DBScan;

namespace FrequentDataMining.Clustering.Common
{
    public abstract class AbstractClusteringAlgorithm
    {
        public int MinPts { get; set; }

        public double Epsilon { get; set; }

        public List<Cluster> Clusters { get; set; }

        public AbstractDistanceFunction DistanceFunction { get; set; }

        public abstract void Run(IEnumerable<IEnumerable<double>> data);
    }
}
