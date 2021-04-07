using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using System.Collections.Generic;

namespace EducationApp.BusinessLogicLayer.Models.Authors
{
    public class AuthorModel : Base.BaseModel
    {
        public string Name { get; set; }
        public List<PrintingEditionModel> PrintingEditions { get; set; }
        public AuthorModel()
        {
            PrintingEditions = new List<PrintingEditionModel>();
        }
    }
}
