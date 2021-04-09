using EducationApp.BusinessLogicLayer.Models.Base;
using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using System.Collections.Generic;

namespace EducationApp.BusinessLogicLayer.Models.Authors
{
    public class AuthorModel : BaseModel
    {
        public string Name { get; set; }
        public List<PrintingEditionModel> PrintingEditions { get; set; }
        public AuthorModel()
        {
            PrintingEditions = new List<PrintingEditionModel>();
        }
    }
}
