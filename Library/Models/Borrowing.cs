using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Borrowing
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBorrowing { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Copy Copy { get; set; }
    }
}
