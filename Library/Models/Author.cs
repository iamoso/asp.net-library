using System.Collections.Generic;

namespace Library.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; }

    }
}
