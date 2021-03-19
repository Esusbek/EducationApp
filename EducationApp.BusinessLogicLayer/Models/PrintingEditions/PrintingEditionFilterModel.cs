using EducationApp.Shared.Enums;
using System.Collections.Generic;

namespace EducationApp.BusinessLogicLayer.Models.PrintingEditions
{
    public class PrintingEditionFilterModel
    {
        public string Title { get; set; }
        public decimal LowPrice { get; set; }
        public decimal HighPrice { get; set; }
        public List<Enums.PrintingEditionType> Type { get; set; }
    }
}
