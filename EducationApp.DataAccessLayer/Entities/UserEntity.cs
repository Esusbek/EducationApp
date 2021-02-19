using Microsoft.AspNetCore.Identity;
using System;

namespace EducationApp.DataAccessLayer.Entities
{
    public class UserEntity : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsRemoved { get; set; }
        public string RefreshToken { get; set; }
    }
}
