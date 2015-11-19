using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrequentDataMining.DBScan;

namespace FrequentDataMining.Common.DistanceFunctions
{
    internal class DistanceEuclidian : DistanceFunction
    {
        public override double CalculateDistance(VectorData a, VectorData b)
        {
            double sum = 0;
            for (var i = 0; i < a.Data.Count(); i++)
            {
                sum += Math.Pow((a.Data[i] - b.Data[i]), 2);
            }

            return Math.Sqrt(sum);
        }
    }
}
