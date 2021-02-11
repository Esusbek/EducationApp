using EducationApp.BusinessLogicLayer.Models.Users;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Helpers
{
    public class CustomUserValidator<TUser> : IUserValidator<TUser> where TUser : UserModel
    {
        public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (user.UserName.Contains("admin"))
            {
                errors.Add(new IdentityError
                {
                    Description = "Ник пользователя не должен содержать слово 'admin'"
                });
            }
            if (!Regex.IsMatch(user.FirstName, @"^[а-яА-Яa-zA-Z]+$") || !Regex.IsMatch(user.LastName, @"^[а-яА-Яa-zA-Z]+$"))
            {
                errors.Add(new IdentityError
                {
                    Description = "Имя и фамилия должны состоять только из букв"
                });
            }
            if (!Regex.IsMatch(user.UserName, @"^[a-zA-Z0-9]+$"))
            {
                errors.Add(new IdentityError
                {
                    Description = "Логин должен состоять только из букв или цифр"
                });
            }

            if (!Regex.IsMatch(user.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                errors.Add(new IdentityError
                {
                    Description = "Невалидная электронная почта"
                });
            }
            return Task.FromResult(errors.Count == 0 ?
                IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
