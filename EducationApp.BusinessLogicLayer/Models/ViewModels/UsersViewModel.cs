using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Models.ViewModels
{
    public class UsersViewModel
    {
        public List<UserModel> Users { get; set; }
        public int CurrentPage { get; set; }
        public int LastPage { get; set; }
        public string SearchString { get; set; }
        public bool GetBlocked { get; set; }
        public bool GetUnblocked { get; set; }

        public UsersViewModel()
        {
            GetUnblocked = true;
            GetBlocked = true;
            CurrentPage = Constants.DEFAULTPAGE;
            SearchString = "";
        }
    }
}
