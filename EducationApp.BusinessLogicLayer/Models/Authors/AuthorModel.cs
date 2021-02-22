using System.Collections.Generic;

namespace EducationApp.BusinessLogicLayer.Models.Authors
{
    public class AuthorModel : Base.BaseModel
    {
        public string Name { get; set; }
        public List<PrintingEditions.PrintingEditionModel> PrintingEditions { get; set; }
        public AuthorModel()
        {
            PrintingEditions = new List<PrintingEditions.PrintingEditionModel>();
        }
    }
}
