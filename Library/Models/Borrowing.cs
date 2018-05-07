using System;

namespace Library.Models
{
    public class Borrowing
    {
        public int Id { get; set; }
        public DateTime DateOfBorrowing { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Copy Copy { get; set; }
    }
}
