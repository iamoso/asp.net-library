﻿namespace Library.Models
{
    public class Copy
    {
        public int Id { get; set; }
        public bool IsAvailable { get; set; }
        public Book Book { get; set; }
    }
}
