using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.UsersViewModels
{
    public class UsersViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
