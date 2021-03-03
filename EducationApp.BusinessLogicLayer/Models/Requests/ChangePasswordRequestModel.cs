using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Models.Requests
{
    public class ChangePasswordRequestModel
    {
        public Users.UserModel User { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
