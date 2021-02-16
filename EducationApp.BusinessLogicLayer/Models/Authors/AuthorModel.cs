using System.Collections.Generic;

namespace EducationApp.BusinessLogicLayer.Models.Authors
{
    public class AuthorModel : Base.BaseModel
    {
        public string Name { get; set; }
        public List<string> PrintingEditions { get; set; }
    }
}
