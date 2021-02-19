namespace EducationApp.BusinessLogicLayer.Models.Requests
{
    public class LoginRequestModel
    {
        public Users.UserModel User { get; set; }
        public bool RememberMe { get; set; }
    }
}
