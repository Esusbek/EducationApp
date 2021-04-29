using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.Shared.Constants;
using System.Collections.Generic;

namespace EducationApp.BusinessLogicLayer.Models.ViewModels
{
    public class UsersViewModel
    {
        public List<UserModel> Users { get; set; }
        public int Page { get; set; }
        public int PageCount { get; set; }
        public string SearchString { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsUnblocked { get; set; }

        public UsersViewModel()
        {
            IsUnblocked = true;
            IsBlocked = true;
            Page = Constants.DEFAULTPAGE;
            SearchString = string.Empty;
        }
    }
}
