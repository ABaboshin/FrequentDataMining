// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Linq;
using FrequentDataMining.AgrawalFaster;
using FrequentDataMining.Common;
using FrequentDataMining.FPGrowth;
using SamplesCommon;

namespace FPGrowthSample
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            
            This sample demonstrate how-to use the FP Growth algorithm.

            As a test data I use a data from an imagine book store:
            1) We have a list of authors, which the store sells.
            2) We have an information about shopping cart of some users.

            As a result we get the following information:
            1. Order by popularity of the authors/set of authors.
            2. What we can advise the user, basing on his shopping cart. It is ordered by probability (frequent).

            */

            TypeRegister.Register<BookAuthor>((a, b) => a.Name.CompareTo(b.Name));

            var data = new SampleHelper();

            var fpGrowth = new FPGrowth<BookAuthor>();
            var result = fpGrowth.ProcessTransactions((double)1/9, data.Transactions);

            foreach (var item in result.OrderByDescending(v => v.Support))
            {
                Console.WriteLine(string.Join("; ", item.Value.OrderBy(i=>i.Name)) + " #SUP: " + item.Support);
            }

            var agrawal = new AgrawalFaster<BookAuthor>();
            var ar = agrawal.Run(0.01, 0.01, result, data.Transactions.Count);
            Console.WriteLine("====");

            foreach (var item in ar.OrderByDescending(r => r.Confidence))
            {
                Console.WriteLine(string.Join("; ", item.Combination) + " => " + string.Join("; ", item.Remaining) + " ===> Confidence: " + item.Confidence + " Lift: " + item.Lift);
            }

            Console.ReadLine();
        }
    }
}
