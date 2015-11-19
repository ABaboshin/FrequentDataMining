using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrequentDataMining.DBScan;

namespace DBScanSample
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

                new List<double> { 8, 7 },
            };

            var dbscan = new DBScan();
            dbscan.MinPts = 2;
            dbscan.Epsilon = 0.29;
            dbscan.Run(points);

            for (var i = 0; i < dbscan.Clusters.Count(); i++)
            {
                System.Diagnostics.Debug.WriteLine("Cluster #" + i);
                foreach (var vector in dbscan.Clusters[i].Vectors)
                {
                    System.Diagnostics.Debug.WriteLine(string.Join(" ", vector));
                }
            }
        }
    }
}
