using EducationApp.Shared.Enums;
using System.Collections.Generic;

namespace EducationApp.BusinessLogicLayer.Models.PrintingEditions
{
    public class PrintingEditionModel : Base.BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public Enums.PrintingEditionStatusType Status { get; set; }
        public Enums.CurrencyType Currency { get; set; }
        public Enums.PrintingEditionType Type { get; set; }

        public List<string> Authors { get; set; }
    }
}
