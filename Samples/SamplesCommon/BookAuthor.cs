// MIT License.
// (c) 2015, Andrey Baboshin

namespace SamplesCommon
{
    public class BookAuthor
    {
        public string Name { get; set; }

        public BookAuthor(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
