// MIT License.
// (c) 2015, Andrey Baboshin

using System.Collections.Generic;

namespace FrequentDataMining.DBScan
{
    public class Cluster
    {
        public List<List<double>> Vectors { get; set; }

        public Cluster()
        {
            Vectors = new List<List<double>>();
        }
    }
}
