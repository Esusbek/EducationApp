using EducationApp.BusinessLogicLayer.Models.Authors;
using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using EducationApp.Shared.Constants;
using System.Collections.Generic;

namespace EducationApp.BusinessLogicLayer.Services.Interfaces
{
    public interface IPrintingEditionService
    {
        public void AddPrintingEdition(PrintingEditionModel printingEdition);
        public void UpdatePrintingEdition(PrintingEditionModel printingEdition);
        public void AddAuthorToPrintingEdition(PrintingEditionModel printingEdition, AuthorModel author);
        public void DeletePrintingEdition(PrintingEditionModel printingEdition);
        public List<PrintingEditionModel> GetPrintingEditionsFiltered(PrintingEditionFilterModel filter = null,
            int page = Constants.Defaults.DefaultPage, bool getRemoved = false);
        public List<PrintingEditionModel> GetPrintingEditions(int page = Constants.Defaults.DefaultPage);
        public PrintingEditionModel GetPrintingEdition(int id);
        public PrintingEditionModel GetPrintingEdition(string title);
    }
}
