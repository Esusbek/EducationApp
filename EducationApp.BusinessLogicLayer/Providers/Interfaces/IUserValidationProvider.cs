using EducationApp.BusinessLogicLayer.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace EducationApp.BusinessLogicLayer.Providers.Interfaces
{
    public interface IUserValidationProvider
    {
        public IdentityResult Validate(UserModel user);
    }
}
