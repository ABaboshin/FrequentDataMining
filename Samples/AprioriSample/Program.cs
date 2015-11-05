// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;
using FrequentDataMining.Apriori;

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
            var ba1 = new BookAuthor("Orwell");
            var ba2 = new BookAuthor("Kafka");
            var ba3 = new BookAuthor("Hesse");
            var ba4 = new BookAuthor("Tucholsky");
            var ba5 = new BookAuthor("Remarque");
            var authors = new List<BookAuthor> {
                ba1,
                ba2,
                ba3,
                ba4,
                ba5
            };

            var readerInterests = new List<List<BookAuthor>> {
                new List<BookAuthor> { ba1, ba2, ba4 },
                new List<BookAuthor> { ba3, ba4, ba5 },
                new List<BookAuthor> { ba1, ba3, ba4 },
                new List<BookAuthor> { ba2, ba3, ba5 },
                new List<BookAuthor> { ba1, ba5 },
                new List<BookAuthor> { ba2, ba4 },
                new List<BookAuthor> { ba4, ba5 },
                new List<BookAuthor> { ba1, ba2, ba3, ba4},
                new List<BookAuthor> { ba2, ba3, ba4, ba5 }
            };

            var result = new Apriori<BookAuthor>().ProcessTransaction(0.01, 0.01, authors, readerInterests);
            foreach (var item in result.FrequentItems.Values.OrderByDescending(v => v.Support))
            {
                Console.WriteLine(string.Join("; ", item.Name) + " " + item.Support);
            }

            Console.WriteLine("====");

            foreach (var item in result.StrongRules.OrderByDescending(r => r.Confidence))
            {
                Console.WriteLine(string.Join("; ", item.Combination) + " => " + string.Join("; ", item.Remaining) + " ===> " + item.Confidence);
            }

            Console.ReadLine();
        }
    }
}
