using EducationApp.BusinessLogicLayer.Models.Authors;
using EducationApp.BusinessLogicLayer.Models.PrintingEditions;

namespace EducationApp.BusinessLogicLayer.Models.Requests
{
    public class AuthorAndEditionRequestModel
    {
        public AuthorModel Author { get; set; }
        public PrintingEditionModel PrintingEdition { get; set; }
    }
}
