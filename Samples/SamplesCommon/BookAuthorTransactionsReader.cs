// MIT License.
// (c) 2015, Andrey Baboshin

using System.Collections.Generic;
using FrequentDataMining.Interfaces;

namespace SamplesCommon
{
    public class BookAuthorTransactionsReader : ITransactionsReader<BookAuthor>
    {
        public IEnumerable<IEnumerable<BookAuthor>> GetTransactions()
        {
            return new SampleHelper().Transactions;
        }
    }
}
