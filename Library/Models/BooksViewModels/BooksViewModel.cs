using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.BooksViewModels
{
    public class BooksViewModel
    {
        public Book Book { get; set; }
        public ICollection<Author> Authors { get; set; }
        public int AvailableCount { get; set; }
    }
}
