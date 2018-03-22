using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Models.UsersViewModels
{
    public class UserEditViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool TwoFactorEnabled { get; set; }

        public IList<string> Roles { get; set; }
        public IList<SelectListItem> RolesToChoose { get; set; }
        public string SelectedRole { get; set; }
    }
}
