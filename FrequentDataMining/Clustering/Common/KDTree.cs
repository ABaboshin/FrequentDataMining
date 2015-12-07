// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;
using FrequentDataMining.Clustering.Common.DistanceFunctions;

namespace FrequentDataMining.Clustering.Common
{
    internal class KDTree
    {
        KDNode root;

        public int DimensionCount { get; set; }

        static Random random = new Random(DateTime.Now.Millisecond);

        public AbstractDistanceFunction AbstractDistanceFunction { get; set; }

        internal void BuildTree(List<VectorData> points)
        {
            if (!points.Any())
            {
                return;
            }

            DimensionCount = points.First().Data.Count();

            root = GenerateNode(0, points, 0, points.Count() - 1);
        }

        internal List<VectorData> PointsWithinRadiusOf(VectorData targetPoint, double radius)
        {
            var result = new List<VectorData>();

            if (root == null)
            {
                return null;
            }

            FindPointsWithinRadius(root, targetPoint, result, radius);

            return result;
        }

        internal List<KNNPoint> PointsWithinRadiusOfWithDistance(VectorData targetPoint, double radius)
        {
            var result = new List<KNNPoint>();

            if (root == null)
            {
                return null;
            }

            FindPointsWithinRadiusWithDistance(root, targetPoint, result, radius);

            return result;
        }

        void FindPointsWithinRadiusWithDistance(KDNode node, VectorData targetPoint, List<KNNPoint> result, double radius)
        {
            if (node.Values != targetPoint)
            {
                TryToSaveRadiusWithDistance(node, targetPoint, result, radius);
            }

            int dMinus1 = node.Dimension - 1;
            if (dMinus1 < 0)
            {
                dMinus1 = DimensionCount - 1;
            }

            double perpendicularDistance = Math.Abs(node.Values.Data.Skip(node.Dimension).First() - targetPoint.Data.Skip(dMinus1).First());
            if (perpendicularDistance < radius || node.Values == targetPoint)
            {
                if (node.Above != null)
                {
                    FindPointsWithinRadiusWithDistance(node.Above, targetPoint, result, radius);
                }
                if (node.Below != null)
                {
                    FindPointsWithinRadiusWithDistance(node.Below, targetPoint, result, radius);
                }
            }
            else
            {
                if (targetPoint.Data.Skip(dMinus1).First() < node.Values.Data.Skip(node.Dimension).First())
                {
                    if (node.Below != null)
                    {
                        FindPointsWithinRadiusWithDistance(node.Below, targetPoint, result, radius);
                    }
                }
                else
                {
                    if (node.Above != null)
                    {
                        FindPointsWithinRadiusWithDistance(node.Above, targetPoint, result, radius);
                    }
                }
            }
        }

        void TryToSaveRadiusWithDistance(KDNode node, VectorData target, List<KNNPoint> result, double radius)
        {
            if (node == null)
            {
                return;
            }
            double distance = AbstractDistanceFunction.CalculateDistance(target, node.Values);
            if (radius < distance)
            {
                return;
            }
            result.Add(new KNNPoint(node.Values, distance));
        }

        void FindPointsWithinRadius(KDNode node, VectorData targetPoint, List<VectorData> result, double radius)
        {
            if (node.Values != targetPoint)
            {
                TryToSaveRadius(node, targetPoint, result, radius);
            }

            var dMinus1 = node.Dimension - 1;
            if (dMinus1 < 0)
            {
                dMinus1 = DimensionCount - 1;
            }

            var perpendicularDistance = Math.Abs(node.Values.Data.Skip(node.Dimension).First() - targetPoint.Data.Skip(dMinus1).First());
            if (perpendicularDistance < radius || node.Values == targetPoint)
            {
                if (node.Above != null)
                {
                    FindPointsWithinRadius(node.Above, targetPoint, result, radius);
                }
                if (node.Below != null)
                {
                    FindPointsWithinRadius(node.Below, targetPoint, result, radius);
                }
            }
            else
            {
                if (targetPoint.Data.Skip(dMinus1).First() < node.Values.Data.Skip(node.Dimension).First())
                {
                    if (node.Below != null)
                    {
                        FindPointsWithinRadius(node.Below, targetPoint, result, radius);
                    }
                }
                else
                {
                    if (node.Above != null)
                    {
                        FindPointsWithinRadius(node.Above, targetPoint, result, radius);
                    }
                }
            }
        }

        void TryToSaveRadius(KDNode node, VectorData target, List<VectorData> result, double radius)
        {
            if (node == null)
            {
                return;
            }

            var distance = AbstractDistanceFunction.CalculateDistance(target, node.Values);
            if (radius < distance)
            {
                return;
            }

            result.Add(node.Values);
        }

        KDNode GenerateNode(int currentD, List<VectorData> points, int left, int right)
        {
            if (right < left)
            {
                return null;
            }

            if (right == left)
            {
                return new KDNode(points[left], currentD);
            }
            
            var m = (right - left) / 2;

            var medianNode = RandomizedSelect(points, m, left, right, currentD);

            var node = new KDNode(medianNode, currentD);
            currentD++;
            if (currentD == DimensionCount)
            {
                currentD = 0;
            }

            node.Below = GenerateNode(currentD, points, left, left + m - 1);
            node.Above = GenerateNode(currentD, points, left + m + 1, right);
            return node;
        }

        VectorData RandomizedSelect(List<VectorData> points, int i, int left,
            int right, int currentD)
        {
            //return points[left + i];
            var p = left;
            var r = right;

            while (true)
            {
                if (p == r)
                {
                    return points[p];
                }

                var q = RandomizedPartition(points, p, r, currentD);
                var k = q - p + 1;

                if (i == k - 1)
                {
                    return points[q];
                }
                else if (i < k)
                {
                    r = q - 1;
                }
                else
                {
                    i = i - k;
                    p = q + 1;
                }
            }
        }

        int RandomizedPartition(List<VectorData> points, int p, int r, int currentD)
        {
            var i = 0;
            if (p < r)
            {
                i = p + random.Next(0, r - p);
            }
            else
            {
                i = r + random.Next(0, p - r);
            }

            Swap(points, r, i);
            return Partition(points, p, r, currentD);
        }

        private int Partition(List<VectorData> points, int p, int r, int currentD)
        {
            var x = points[r];
            var i = p - 1;
            for (var j = p; j <= r - 1; j++)
            {
                if (points[j].Data.Skip(currentD).First() <= x.Data.Skip(currentD).First())
                {
                    i = i + 1;
                    Swap(points, i, j);
                }
            }

            Swap(points, i + 1, r);
            return i + 1;
        }

        private void Swap(List<VectorData> points, int i, int j)
        {
            var tmp = points[i];
            points[i] = points[j];
            points[j] = tmp;
        }
    }
}
