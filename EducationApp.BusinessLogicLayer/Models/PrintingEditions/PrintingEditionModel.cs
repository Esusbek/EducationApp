using EducationApp.Shared.Enums;

namespace EducationApp.BusinessLogicLayer.Models.PrintingEditions
{
    public class PrintingEditionModel : Base.BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public Enums.PrintingEdition.Status Status { get; set; }
        public Enums.Currency Currency { get; set; }
        public Enums.PrintingEdition.Type Type { get; set; }
    }
}
