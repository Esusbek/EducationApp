using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationApp.DataAccessLayer.Entities
{
    [Table("AspNetUsers")]
    public class UserEntity : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsRemoved { get; set; }
        public string RefreshToken { get; set; }
        public string ProfilePictureURL { get; set; }
        public string ProfilePictureStorageName { get; set; }
    }
}
