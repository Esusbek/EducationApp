namespace EducationApp.BusinessLogicLayer.Models.Requests
{
    public class ResetPasswordRequestModel
    {
        public string UserId { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }
    }

}
