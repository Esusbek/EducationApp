
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace EducationApp.DataAccessLayer.Entities
{
    public class UserEntity: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
