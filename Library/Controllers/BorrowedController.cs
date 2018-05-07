using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Data;
using Library.Models;
using Library.Models.BorrowedViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class BorrowedController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LibraryContext _context;

        public BorrowedController(UserManager<ApplicationUser> userManager, LibraryContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var borrowings = _context.Borrowings.Where(u => u.ApplicationUser.Id == userId)
                .Include(b => b.Copy)
                .Include(b => b.Copy.Book)
                .ToList();

            var model = borrowings.Select(borrowing => new BorrowedViewModel
                {
                    Borrowing = borrowing,
                    Book = borrowing.Copy.Book
                })
                .ToList();

            return View(model);
        }
    }
}