// MIT License.
// (c) 2015, Andrey Baboshin

using System.Collections.Generic;

namespace FrequentDataMining.Interfaces
{
    /// <summary>
    /// Transaction Reader
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITransactionsReader<T>
    {
        /// <summary>
        /// get transactions
        /// </summary>
        /// <returns></returns>
        IEnumerable<IEnumerable<T>> GetTransactions();
    }
}
