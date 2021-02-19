using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.Shared.Constants;
using EducationApp.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace EducationApp.BusinessLogicLayer.Providers
{
    public class UserValidationProvider : Interfaces.IUserValidationProvider
    {

        private readonly ProfanityFilter.ProfanityFilter _censor;
        public UserValidationProvider()
        {
            var bannedWords = Constants.BANNEDWORDSVALIDATOR.Split(',').ToList();
            _censor = new ProfanityFilter.ProfanityFilter(bannedWords);
        }
        public IdentityResult Validate(UserModel user)
        {
            List<string> errors = new List<string>();
            if (!user.PasswordConfirm.Equals(user.Password))
            {
                errors.Add(Constants.INVALIDPASSWORDDONOTMATCH);
            }
            if (_censor.DetectAllProfanities(user.UserName).Any())
            {
                errors.Add(Constants.INVALIDHASBANNEDWORDS);
            }
            if (!Regex.IsMatch(user.FirstName, Constants.NAMEVALIDATOR) || !Regex.IsMatch(user.LastName, Constants.NAMEVALIDATOR))
            {
                errors.Add(Constants.INVALIDNAME);
            }
            if (!Regex.IsMatch(user.UserName, Constants.USERNAMEVALIDATOR))
            {
                errors.Add(Constants.INVALIDUSERNAME);
            }

            if (!Regex.IsMatch(user.Email, Constants.EMAILVALIDATOR))
            {
                errors.Add(Constants.INVALIDEMAIL);
            }
            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    user.Errors.Add(error);
                }
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.INCORRECTINPUTERROR);
            }
            return IdentityResult.Success;
        }
    }
}
