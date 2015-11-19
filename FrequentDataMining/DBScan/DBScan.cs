// MIT License.
// (c) 2015, Andrey Baboshin

using System.Collections.Generic;
using System.Linq;

namespace FrequentDataMining.DBScan
{
    public class DBScan
    {
        public int MinPts { get; set; }

        public double Epsilon { get; set; }

        public List<Cluster> Clusters { get; set; }

        public long NumberOfNoisePoints { get; set; }

        public void Run(IEnumerable<IEnumerable<double>> data)
        {
            NumberOfNoisePoints = 0;
            var points = data.Select(d => new VectorData {Data = d.ToList()}).ToList();
            kdTree = new KDTree();
            kdTree.BuildTree(points);

            Clusters = new List<Cluster>();

            foreach (var point in points)
            {
                if (point.Visited)
                {
                    continue;
                }

                point.Visited = true;

                var neighboors = kdTree.PointsWithinRadiusOf(point, Epsilon);

                if (neighboors.Count >= MinPts - 1)
                { 
                    var cluster = new Cluster();
                    Clusters.Add(cluster);

                    ExpandCluster(point, neighboors, cluster, Epsilon, MinPts);
                }
                else
                {
                    NumberOfNoisePoints++;
                }
            }
        }

        KDTree kdTree;

        private void ExpandCluster(VectorData currentPoint,
            List<VectorData> neighboors, Cluster cluster, double epsilon, int minPts)
        {
            cluster.Vectors.Add(currentPoint.Data);

            foreach (var newPoint in neighboors)
            {
                if (!newPoint.Visited)
                {
                    newPoint.Visited = true;

                    var newNeighboors = kdTree.PointsWithinRadiusOf(newPoint, epsilon);

                    if (newNeighboors.Count >= minPts - 1)
                    {
                        ExpandCluster(newPoint, newNeighboors, cluster, epsilon, minPts);
                    }
                    else
                    {
                        NumberOfNoisePoints++;
                    }
                }
            }
        }
    }
}
