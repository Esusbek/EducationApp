using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.Shared.Constants;
using System.Collections.Generic;

namespace EducationApp.BusinessLogicLayer.Models.ViewModels
{
    public class UsersViewModel
    {
        public List<UserModel> Users { get; set; }
        public int Page { get; set; }
        public int LastPage { get; set; }
        public string SearchString { get; set; }
        public bool GetBlocked { get; set; }
        public bool GetUnblocked { get; set; }

        public UsersViewModel()
        {
            GetUnblocked = true;
            GetBlocked = true;
            Page = Constants.DEFAULTPAGE;
            SearchString = string.Empty;
        }
    }
}
