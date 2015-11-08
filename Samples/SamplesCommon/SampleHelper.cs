// MIT License.
// (c) 2015, Andrey Baboshin

using System.Collections.Generic;

namespace SamplesCommon
{
    /// <summary>
    /// sample helper
    /// </summary>
    public class SampleHelper
    {
        /// <summary>
        /// items
        /// </summary>
        public List<BookAuthor> Items { get; set; }

        /// <summary>
        /// transaction
        /// </summary>
        public List<List<BookAuthor>> Transactions { get; set; }

        public SampleHelper()
        {
            var ba1 = new BookAuthor("Orwell");
            var ba2 = new BookAuthor("Kafka");
            var ba3 = new BookAuthor("Hesse");
            var ba4 = new BookAuthor("Tucholsky");
            var ba5 = new BookAuthor("Remarque");

            Items = new List<BookAuthor> {
                ba1,
                ba2,
                ba3,
                ba4,
                ba5
            };

            Transactions = new List<List<BookAuthor>> {
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
        }
    }
}
