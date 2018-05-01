using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Data;
using Library.Models;
using Library.Models.BooksViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LibraryContext _context;

        public BooksController(UserManager<ApplicationUser> userManager, LibraryContext context)
        {
            _userManager = userManager;
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

        [HttpPost]
        [Authorize(Roles = "Administrator, User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Borrow(int id)
        {
            var copy = _context.Copies.FirstOrDefault(c => c.Book.Id == id && c.IsAvailable);

            if (copy != null)
            {
                var borrowing = new Borrowing
                {
                    Copy = copy,
                    DateOfBorrowing = DateTime.Today,
                    ApplicationUser = await _userManager.GetUserAsync(User)
                };

                await _context.Borrowings.AddAsync(borrowing);
                copy.IsAvailable = false;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}