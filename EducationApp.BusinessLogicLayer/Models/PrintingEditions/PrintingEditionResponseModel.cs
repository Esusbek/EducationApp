using System.Collections.Generic;

namespace EducationApp.BusinessLogicLayer.Models.PrintingEditions
{
    public class PrintingEditionResponseModel
    {
        public List<PrintingEditionModel> Books { get; set; }
        public PrintingEditionsInfoModel Info { get; set; }
    }
}
