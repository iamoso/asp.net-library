using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.BorrowedViewModels
{
    public class BorrowedViewModel
    {
        public Borrowing Borrowing { get; set; }
        public Book Book { get; set; }
    }
}
