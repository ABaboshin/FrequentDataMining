// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Linq;
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

            */

            var data = new SampleHelper();

            var fpGrowth = new FPGrowth<BookAuthor>();
            fpGrowth.ProcessTransactions((double)1/9, data.Transactions);

            foreach (var item in fpGrowth.Result.OrderByDescending(v => v.Support))
            {
                Console.WriteLine(string.Join("; ", item.Value.OrderBy(i=>i.Name)) + " #SUP: " + item.Support);
            }

            Console.ReadLine();
        }
    }
}
