using EducationApp.BusinessLogicLayer.Models.Users;

namespace EducationApp.BusinessLogicLayer.Models.ViewModels
{
    public class ProfileViewModel
    {
        public UserModel User { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
