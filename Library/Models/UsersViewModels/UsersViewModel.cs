using System.Collections.Generic;

namespace Library.Models.UsersViewModels
{
    public class UsersViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
