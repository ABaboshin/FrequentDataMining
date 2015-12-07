// MIT License.
// (c) 2015, Andrey Baboshin

using System.Collections.Generic;

namespace FrequentDataMining.Clustering.Common
{
    public class VectorData
    {
        public IEnumerable<double> Data { get; set; }

        public bool Visited { get; set; }

        public override string ToString()
        {
            return string.Join(" ", Data);
        }
    }
}
