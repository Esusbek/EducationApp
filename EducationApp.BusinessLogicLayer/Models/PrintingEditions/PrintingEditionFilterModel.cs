using EducationApp.Shared.Enums;

namespace EducationApp.BusinessLogicLayer.Models.PrintingEditions
{
    public class PrintingEditionFilterModel
    {
        public string Title { get; set; }
        public decimal LowPrice { get; set; }
        public decimal HighPrice { get; set; }
        public Enums.PrintingEdition.Type Type { get; set; }
    }
}
