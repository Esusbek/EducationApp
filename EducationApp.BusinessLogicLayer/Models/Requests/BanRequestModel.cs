namespace EducationApp.BusinessLogicLayer.Models.Requests
{
    public class BanRequestModel
    {
        public Users.UserModel User { get; set; }
        public int Duration { get; set; }
    }
}
