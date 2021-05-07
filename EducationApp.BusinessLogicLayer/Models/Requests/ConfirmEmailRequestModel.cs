namespace EducationApp.BusinessLogicLayer.Models.Requests
{
    public class ConfirmEmailRequestModel
    {
        public string UserId { get; set; }
        public string Code { get; set; }
    }
}
