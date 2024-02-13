// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;
using FrequentDataMining.Clustering.Common.DistanceFunctions;
using FrequentDataMining.Clustering.OPTICS;

namespace OpticsSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var points = new List<List<double>>
            {
                new List<double> { 0.5, 0.5 },
                new List<double> { 0.4, 0.4 },
                new List<double> { 0.6, 0.6 },
                new List<double> { 1.4, 0.5 },
                new List<double> { 1.4, 0.4 },
                new List<double> { 1.4, 0.6 },

                new List<double> { 1.5, 2.5 },
                new List<double> { 1.5, 2.4 },
                new List<double> { 1.5, 2.6 },
                new List<double> { 1.6, 2.5 },
                new List<double> { 1.6, 2.4 },
                new List<double> { 1.6, 2.6 },

                new List<double> { 8, 7 },
            };

            var optics = new OPTICS();
            optics.DistanceFunction = new DistanceEuclidian();
            optics.MinPts = 3;
            optics.Epsilon = 0.4;
            optics.Run(points);

            for (var i = 0; i < optics.Clusters.Count(); i++)
            {
                Console.WriteLine("Cluster #" + i);
                foreach (var vector in optics.Clusters[i].Vectors)
                {
                    Console.WriteLine(string.Join(" ", vector));
                }
            }

            Console.ReadLine();
        }
    }
}
