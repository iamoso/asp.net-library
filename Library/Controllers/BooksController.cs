using System.Collections.Generic;
using System.Linq;
using Library.Data;
using Library.Models;
using Library.Models.BooksViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new List<BooksViewModel>();
            var books = _context.Books.ToList();

            foreach (var book in books)
            {
                var authors = _context.BookAuthors.Where(b => b.BookId == book.Id).ToList();
                var bookAuthors = authors.Select(author => _context.Authors.SingleOrDefault(a => a.Id == author.AuthorId)).ToList();
                //var count = _context.Copies.Select(c => c.Book.Id == book.Id && c.IsAvailable).ToList().Count;
                var count = _context.Copies.Count(c => c.Book.Id == book.Id && c.IsAvailable);

                model.Add(new BooksViewModel
                {
                    Book = book,
                    Authors = bookAuthors,
                    AvailableCount = count
                });
            }

            return View(model);
        }
    }
}