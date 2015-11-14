// MIT License.
// (c) 2015, Andrey Baboshin

using FrequentDataMining.Common;

namespace FrequentDataMining.Interfaces
{
    /// <summary>
    /// Itemset Writer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IItemsetWriter<T>
    {
        /// <summary>
        /// writer a itemset
        /// </summary>
        /// <param name="itemset"></param>
        void SaveItemset(Itemset<T> itemset);
    }
}
