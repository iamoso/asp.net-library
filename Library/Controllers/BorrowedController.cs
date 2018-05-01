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

        public async Task<IActionResult> Index()
        {
            var model = new List<BorrowedViewModel>();
            var borrowings = _context.Borrowings.Include(b => b.ApplicationUser).ToList();
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);

            foreach (var borrowing in borrowings)
            {
                if (borrowing.ApplicationUser.Id == userId)
                {
                    var book = borrowing.Copy.Book;

                    model.Add(new BorrowedViewModel
                    {
                        Borrowing = borrowing,
                        Book = book
                    });
                }
            }

            return View(model);
        }
    }
}