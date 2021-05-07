using EducationApp.BusinessLogicLayer.Models.Users;

namespace EducationApp.BusinessLogicLayer.Models.Requests
{
    public class BanRequestModel
    {
        public UserModel User { get; set; }
        public int Duration { get; set; }
    }
}
