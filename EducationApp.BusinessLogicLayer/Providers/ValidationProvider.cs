using EducationApp.BusinessLogicLayer.Models.Authors;
using EducationApp.BusinessLogicLayer.Models.Orders;
using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.BusinessLogicLayer.Providers.Interfaces;
using EducationApp.Shared.Constants;
using EducationApp.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace EducationApp.BusinessLogicLayer.Providers
{
    public class ValidationProvider : IValidationProvider
    {

        private readonly ProfanityFilter.ProfanityFilter _censor;
        public ValidationProvider()
        {
            var bannedWords = Constants.BANNEDWORDSVALIDATOR.Split(',').ToList();
            _censor = new ProfanityFilter.ProfanityFilter(bannedWords);
        }

        public void ValidateAuthor(AuthorModel author)
        {
            if (string.IsNullOrWhiteSpace(author.Name))
            {
                author.Errors.Add(Constants.INVALIDEMPTYINPUT);
            }
            if (author.Errors.Any())
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.INCORRECTINPUTERROR);
            }
        }

        public void ValidateOrder(OrderModel order)
        {
            if (!order.CurrentItems.Any())
            {
                order.Errors.Add(Constants.INVALIDEMPTYORDER);
            }
            if (string.IsNullOrWhiteSpace(order.UserId))
            {
                order.Errors.Add(Constants.INVALIDUSERID);
            }
            if (order.Errors.Any())
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.INCORRECTINPUTERROR);
            }
        }

        public void ValidatePrintingEdition(PrintingEditionModel printingEdition)
        {
            if (string.IsNullOrWhiteSpace(printingEdition.Title))
            {
                printingEdition.Errors.Add(Constants.INVALIDEMPTYINPUT);
            }
            if (printingEdition.Price == default)
            {
                printingEdition.Errors.Add(Constants.INVALIDEMPTYINPUT);
            }
            if (printingEdition.Currency == default)
            {
                printingEdition.Errors.Add(Constants.INVALIDEMPTYINPUT);
            }
            if (printingEdition.Type == default)
            {
                printingEdition.Errors.Add(Constants.INVALIDEMPTYINPUT);
            }
            if (printingEdition.Errors.Any())
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.INCORRECTINPUTERROR);
            }
        }

        public IdentityResult ValidateUser(UserModel user)
        {
            if (!user.PasswordConfirm.Equals(user.Password))
            {
                user.Errors.Add(Constants.INVALIDPASSWORDDONOTMATCH);
            }
            if (_censor.DetectAllProfanities(user.UserName).Any())
            {
                user.Errors.Add(Constants.INVALIDHASBANNEDWORDS);
            }
            if (!Regex.IsMatch(user.FirstName, Constants.NAMEVALIDATOR) || !Regex.IsMatch(user.LastName, Constants.NAMEVALIDATOR))
            {
                user.Errors.Add(Constants.INVALIDNAME);
            }
            if (!Regex.IsMatch(user.UserName, Constants.USERNAMEVALIDATOR))
            {
                user.Errors.Add(Constants.INVALIDUSERNAME);
            }

            if (!Regex.IsMatch(user.Email, Constants.EMAILVALIDATOR))
            {
                user.Errors.Add(Constants.INVALIDEMAIL);
            }
            if (user.Errors.Any())
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.INCORRECTINPUTERROR);
            }
            return IdentityResult.Success;
        }

    }
}
