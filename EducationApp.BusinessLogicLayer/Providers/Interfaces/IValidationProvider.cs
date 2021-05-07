using EducationApp.BusinessLogicLayer.Models.Authors;
using EducationApp.BusinessLogicLayer.Models.Orders;
using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using EducationApp.BusinessLogicLayer.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace EducationApp.BusinessLogicLayer.Providers.Interfaces
{
    public interface IValidationProvider
    {
        public IdentityResult ValidateUser(UserModel user);
        public void ValidateAuthor(AuthorModel author);
        public void ValidatePrintingEdition(PrintingEditionModel printingEdition);
        public void ValidateOrder(OrderModel order);
    }
}
