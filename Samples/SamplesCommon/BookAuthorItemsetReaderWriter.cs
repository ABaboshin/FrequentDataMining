// MIT License.
// (c) 2015, Andrey Baboshin

using System.Collections.Generic;
using FrequentDataMining.Common;
using FrequentDataMining.Interfaces;

namespace SamplesCommon
{
    public class BookAuthorItemsetReaderWriter : IItemsetWriter<BookAuthor>, IItemsetReader<BookAuthor>
    {
        public List<Itemset<BookAuthor>> Itemsets { get; set; }

        public BookAuthorItemsetReaderWriter()
        {
            Itemsets = new List<Itemset<BookAuthor>>();
        }

        public void SaveItemset(Itemset<BookAuthor> itemset)
        {
            Itemsets.Add(itemset);
        }

        public IEnumerable<Itemset<BookAuthor>> GetItemsets()
        {
            return Itemsets;
        }
    }
}
