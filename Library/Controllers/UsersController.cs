using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Models;
using Library.Models.UsersViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new List<UsersViewModel>();
            var users = await _userManager.GetUsersInRoleAsync("User");

            foreach (var user in users)
            {
                model.Add(new UsersViewModel
                {
                    ApplicationUser = user,
                    Roles = await _userManager.GetRolesAsync(user)
                });
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            var model = new UserEditViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                LockoutEnd = user.LockoutEnd,
                TwoFactorEnabled = user.TwoFactorEnabled
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.EmailConfirmed = model.EmailConfirmed;
                user.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
                user.LockoutEnd = model.LockoutEnd;
                user.TwoFactorEnabled = model.TwoFactorEnabled;

                _userManager.UpdateAsync(user).Wait();

                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}