using System;

namespace Library.Models
{
    public class Borrowing
    {
        public int Id { get; set; }
        public DateTime DateOfBorrowing { get; set; }

        //public User User { get; set; }
        public Copy Copy { get; set; }

    }
}
