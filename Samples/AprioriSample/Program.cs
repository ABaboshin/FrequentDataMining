// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Linq;
using FrequentDataMining.AgrawalFaster;
using FrequentDataMining.Apriori;
using FrequentDataMining.Common;
using SamplesCommon;

namespace AprioriSample
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            
            This sample demonstrate how-to use the Apriori algorithm.

            As a test data I use a data from an imagine book store:
            1) We have a list of authors, which the store sells.
            2) We have an information about shopping cart of some users.

            As a result we get the following information:
            1. Order by popularity of the authors/set of authors.
            2. What we can advise the user, basing on his shopping cart. It is ordered by probability (frequent).

            */

            TypeRegister.Register<BookAuthor>((a, b) => a.Name.CompareTo(b.Name));

            var itemsetReaderWriter = new BookAuthorItemsetReaderWriter();
            var transactionsReader = new BookAuthorTransactionsReader();

            var apriori = new Apriori<BookAuthor>
            {
                MinSupport = (double) 1/9,
                ItemsetWriter = itemsetReaderWriter,
                TransactionsReader = transactionsReader
            };

            apriori.ProcessTransactions();

            var ruleWriter = new BookAuthorRuleWriter();

            var agrawal = new AgrawalFaster<BookAuthor>
            {
                MinLift = 0.01,
                MinConfidence = 0.01,
                TransactionsCount = transactionsReader.GetTransactions().Count(),
                ItemsetReader = itemsetReaderWriter,
                RuleWriter = ruleWriter
            };

            agrawal.Run();

            foreach (var item in itemsetReaderWriter.Itemsets.OrderByDescending(v => v.Support))
            {
                Console.WriteLine(string.Join("; ", item.Value.OrderBy(i => i.Name)) + " #SUP: " + item.Support);
            }

            Console.WriteLine("====");

            foreach (var item in ruleWriter.Rules.OrderByDescending(r => r.Confidence))
            {
                Console.WriteLine(string.Join("; ", item.Combination) + " => " + string.Join("; ", item.Remaining) + " ===> Confidence: " + item.Confidence + " Lift: " + item.Lift);
            }

            Console.ReadLine();
        }
    }
}
