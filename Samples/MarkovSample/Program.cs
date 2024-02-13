// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using FrequentDataMining.Common;
using FrequentDataMining.Markov;
using SamplesCommon;

namespace MarkovSample
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            This sample demonstrate how-to use the Markov prediction algorithm.
            
            */

            TypeRegister.Register<BookAuthor>((a, b) => a.Name.CompareTo(b.Name));

            var markov = new MarkovPredictor<BookAuthor> {K = 5};
            markov.Train(()=> new SampleHelper().Transactions);
            var sequence = markov.Predict(new List<BookAuthor>
                {
                    new BookAuthor("Hesse"),
                    new BookAuthor("Tucholsky"),
                });

            foreach (var bookAuthor in sequence)
            {
                Console.WriteLine(bookAuthor.Name);
            }

            Console.ReadLine();
        }
    }
}
