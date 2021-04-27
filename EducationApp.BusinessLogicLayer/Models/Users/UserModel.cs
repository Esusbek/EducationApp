using EducationApp.BusinessLogicLayer.Models.Base;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

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
        [NotMapped]
        public virtual IFormFile ProfilePicture { get; set; }
        public string ProfilePictureStorageName { get; set; }
        public string ProfilePictureURL { get; set; }
    }
}
