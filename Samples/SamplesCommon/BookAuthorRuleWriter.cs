// MIT License.
// (c) 2015, Andrey Baboshin

using System.Collections.Generic;
using FrequentDataMining.AgrawalFaster;
using FrequentDataMining.Interfaces;

namespace SamplesCommon
{
    public class BookAuthorRuleWriter : IRuleWriter<BookAuthor>
    {
        public List<Rule<BookAuthor>> Rules { get; set; }

        public BookAuthorRuleWriter()
        {
            Rules = new List<Rule<BookAuthor>>();
        }

        public void SaveRule(Rule<BookAuthor> rule)
        {
            Rules.Add(rule);
        }
    }
}
