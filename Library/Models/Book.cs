﻿using System.Collections.Generic;

namespace Library.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
