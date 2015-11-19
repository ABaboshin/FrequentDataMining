// MIT License.
// (c) 2015, Andrey Baboshin

namespace FrequentDataMining.DBScan
{
    internal class KDNode
    {
        internal VectorData Values { get; set; }

        internal int Dimension { get; set; }

        internal KDNode Above { get; set; }

        internal KDNode Below { get; set; }

        internal KDNode(VectorData values, int dimension)
        {
            Values = values;
            Dimension = dimension;
        }
    }
}
