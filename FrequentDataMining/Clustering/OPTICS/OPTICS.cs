// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;
using FrequentDataMining.Clustering.Common;
using FrequentDataMining.Clustering.DBScan;

namespace FrequentDataMining.Clustering.OPTICS
{
    public class OPTICS : AbstractClusteringAlgorithm
    {
        public override void Run(IEnumerable<IEnumerable<double>> data)
        {
            ComputerClusterOrdering(data);
            ExtractDBScan();
        }

        void ExtractDBScan() {
            Clusters = new List<Cluster>();

            var currentCluster = new Cluster();

            foreach (var op in clusterOrdering)
            {
                if (op.ReachabilityDistance > Epsilon && !(currentCluster.Vectors.Any() && DistanceFunction.CalculateDistance(new VectorData {Data = currentCluster.Vectors.First() }, op) <= Epsilon))
                {
                    if (op.CoreDistance <= Epsilon)
                    {
                        if (currentCluster.Vectors.Any())
                        {
                            Clusters.Add(currentCluster);
                        }

                        currentCluster = new Cluster();
                        currentCluster.Vectors.Add(op.Data);
                    }
                }
                else
                {
                    currentCluster.Vectors.Add(op.Data);
                }
            }

            if (currentCluster.Vectors.Any())
            {
                Clusters.Add(currentCluster);
            }

        }

        KDTree kdTree;
        private List<VectorDataOptics> clusterOrdering;

        private void ComputerClusterOrdering(IEnumerable<IEnumerable<double>> data)
        {
            var points = data.Select(d => new VectorDataOptics {Data = d.ToList()} as VectorData).ToList();
            if (points.Any() && points.First().Data.Count() >= 2)
            {
                points = points.OrderBy(p => p.Data.First()).ThenBy(p => p.Data.Skip(1).First()).ToList();
            }

            kdTree = new KDTree();
            kdTree.AbstractDistanceFunction = DistanceFunction;
            kdTree.BuildTree(points);

            clusterOrdering = new List<VectorDataOptics>();

            foreach (var point in points.OfType<VectorDataOptics>())
            {
                if (!point.Visited)
                {
                    ExpandClusterOrder(point);
                }
            }

            kdTree = null;
        }

        private void ExpandClusterOrder(VectorDataOptics pointDBS)
        {
            var neighbors = kdTree.PointsWithinRadiusOfWithDistance(pointDBS, Epsilon);

            pointDBS.Visited = true;

            pointDBS.ReachabilityDistance = double.PositiveInfinity;

            pointDBS.SetCoreDistance(neighbors, Epsilon, MinPts);

            clusterOrdering.Add(pointDBS);

            if (!double.IsPositiveInfinity(pointDBS.CoreDistance))
            {
                var orderSeeds = new PriorityQueue<VectorDataOptics>();
                Update(neighbors, pointDBS, orderSeeds);
                while (orderSeeds.IsEmpty() == false)
                {
                    var currentObject = orderSeeds
                            .Poll();

                    var neighborsCurrent = kdTree
                            .PointsWithinRadiusOfWithDistance(pointDBS, Epsilon);

                    currentObject.Visited = true;

                    currentObject.SetCoreDistance(neighborsCurrent, Epsilon, MinPts);

                    clusterOrdering.Add(currentObject);

                    if (!double.IsPositiveInfinity(currentObject.CoreDistance))
                    {
                        Update(neighborsCurrent, currentObject, orderSeeds);
                    }
                }
            }
        }

        private void Update(List<KNNPoint> neighbors,
            VectorDataOptics centerObject, PriorityQueue<VectorDataOptics> orderSeeds)
        {
            var cDist = centerObject.CoreDistance;

            foreach (var obj in neighbors)
            {
                var op = obj.Data as VectorDataOptics;
                if (!op.Visited)
                {
                    var newRDistance = Math.Max(cDist, DistanceFunction
                            .CalculateDistance(op, centerObject));

                    if (double.IsPositiveInfinity(op.ReachabilityDistance))
                    {
                        op.ReachabilityDistance = newRDistance;
                        orderSeeds.Add(op);
                    }
                    else
                    {
                        if (newRDistance < op.ReachabilityDistance)
                        {
                            op.ReachabilityDistance = newRDistance;
                            orderSeeds.Remove(op);
                            orderSeeds.Add(op);
                        }
                    }
                }
            }
        }
    }
}
