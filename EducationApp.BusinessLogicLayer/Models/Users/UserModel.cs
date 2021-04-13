using EducationApp.BusinessLogicLayer.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace EducationApp.BusinessLogicLayer.Models.Users
{
    public class UserModel : BaseErrorsModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsRemoved { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }

    }
}
