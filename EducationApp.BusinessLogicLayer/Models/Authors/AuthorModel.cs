using EducationApp.BusinessLogicLayer.Models.Base;
using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EducationApp.BusinessLogicLayer.Models.Authors
{
    public class AuthorModel : BaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public List<PrintingEditionModel> PrintingEditions { get; set; }
        public AuthorModel()
        {
            PrintingEditions = new List<PrintingEditionModel>();
        }
    }
}
