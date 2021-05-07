using EducationApp.BusinessLogicLayer.Models.Users;

namespace EducationApp.BusinessLogicLayer.Models.Requests
{
    public class ChangePasswordRequestModel
    {
        public UserModel User { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
