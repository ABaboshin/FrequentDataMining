using System;

namespace AprioriSample
{
    public class BookAuthor : IComparable<BookAuthor>, IEquatable<BookAuthor>
    {
        public string Name { get; set; }

        public BookAuthor(string name)
        {
            Name = name;
        }

        public int CompareTo(BookAuthor other)
        {
            return Name.CompareTo(other.Name);
        }

        public bool Equals(BookAuthor other)
        {
            return Name.Equals(other.Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
