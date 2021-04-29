namespace EducationApp.BusinessLogicLayer.Models.Requests
{
    public class ChangeAdminPasswordRequestModel
    {
        public string Id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
