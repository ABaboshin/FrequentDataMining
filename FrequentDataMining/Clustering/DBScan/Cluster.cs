// MIT License.
// (c) 2015, Andrey Baboshin

using System.Collections.Generic;

namespace FrequentDataMining.Clustering.DBScan
{
    public class Cluster
    {
        public List<IEnumerable<double>> Vectors { get; set; }

        public Cluster()
        {
            Vectors = new List<IEnumerable<double>>();
        }
    }
}
