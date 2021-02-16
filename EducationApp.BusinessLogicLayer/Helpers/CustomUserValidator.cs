using Censored;
using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Helpers
{
    public class CustomUserValidator<TUser> : IUserValidator<TUser> where TUser : UserModel
    {
        private readonly Censor _censor;
        public CustomUserValidator()
        {
            var bannedWords = Constants.Validators.BannedWords.Split(',').ToList();
            _censor = new Censor(bannedWords);
        }
        public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            List<IdentityError> errors = new List<IdentityError>();
            if (!user.PasswordConfirm.Equals(user.Password))
            {
                errors.Add(new IdentityError
                {
                    Description = Constants.ValidationErrors.PasswordDoNotMatch
                });
            }
            if (_censor.HasCensoredWord(user.UserName))
            {
                errors.Add(new IdentityError
                {
                    Description = Constants.ValidationErrors.HasBannedWords
                });
            }
            if (!Regex.IsMatch(user.FirstName, Constants.Validators.NameValidator) || !Regex.IsMatch(user.LastName, Constants.Validators.NameValidator))
            {
                errors.Add(new IdentityError
                {
                    Description = Constants.ValidationErrors.InvalidName
                });
            }
            if (!Regex.IsMatch(user.UserName, Constants.Validators.UsernameValidator))
            {
                errors.Add(new IdentityError
                {
                    Description = Constants.ValidationErrors.InvalidUsername
                });
            }

            if (!Regex.IsMatch(user.Email, Constants.Validators.EmailValidator))
            {
                errors.Add(new IdentityError
                {
                    Description = Constants.ValidationErrors.InvalidEmail
                });
            }
            return Task.FromResult(errors.Count == 0 ?
                IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
