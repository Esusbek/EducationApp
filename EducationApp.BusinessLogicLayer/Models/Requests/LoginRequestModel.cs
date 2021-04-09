using EducationApp.BusinessLogicLayer.Models.Users;

namespace EducationApp.BusinessLogicLayer.Models.Requests
{
    public class LoginRequestModel
    {
        public UserModel User { get; set; }
        public bool RememberMe { get; set; }
    }
}
