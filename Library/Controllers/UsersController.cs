using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Library.Models;
using Library.Models.UsersViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly List<SelectListItem> _rolesList;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            
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
            Debug.WriteLine("get edit");

            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);

            var userRoles = await _userManager.GetRolesAsync(user);
            var rolesList = _roleManager.Roles.ToList().Select(role => new SelectListItem { Text = role.Name, Value = role.Name }).ToList();

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
                TwoFactorEnabled = user.TwoFactorEnabled,
                Roles = userRoles,
                RolesToChoose = rolesList
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View();

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

        [Route("Users/AddRole/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(nameof(Edit));

            var user = await _userManager.FindByIdAsync(model.Id);

            await _userManager.AddToRoleAsync(user, model.SelectedRole);

            return RedirectToAction("Edit", new {id = model.Id});
        }

        [Route("Users/DeleteRole/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(UserEditViewModel model)
        {
            if(!ModelState.IsValid)
                return View(nameof(Edit));

            var user = await _userManager.FindByIdAsync(model.Id);

            await _userManager.RemoveFromRoleAsync(user, model.SelectedRole);

            return RedirectToAction("Edit", new { id = model.Id });
        }
    }
}