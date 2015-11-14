// MIT License.
// (c) 2015, Andrey Baboshin

using System.Collections.Generic;
using FrequentDataMining.Common;

namespace FrequentDataMining.Interfaces
{
    /// <summary>
    /// Itemset Reader 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IItemsetReader<T>
    {
        /// <summary>
        /// get itemsets
        /// </summary>
        /// <returns></returns>
        IEnumerable<Itemset<T>> GetItemsets();
    }
}
