using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.FilterModels;
using EducationApp.Shared.Constants;
using System.Collections.Generic;

namespace EducationApp.BusinessLogicLayer.Services.Interfaces
{
    public interface IPrintingEditionService
    {
        public void AddPrintingEdition(PrintingEditionModel printingEdition);
        public void UpdatePrintingEdition(PrintingEditionModel printingEdition);
        public void DeletePrintingEdition(PrintingEditionModel printingEdition);
        public List<PrintingEditionModel> GetPrintingEditionsAdmin(bool getBook = true, bool getNewspaper = true, bool getJournal = true, string field = Constants.DEFAULTEDITIONSORT, string orderAsc = Constants.DEFAULTSORTORDER, int page = Constants.DEFAULTPAGE, bool getRemoved = false);
        public int GetLastPage(bool getBook = true, bool getNewspaper = true, bool getJournal = true);
        public PrintingEditionResponseModel GetPrintingEditionsFiltered(PrintingEditionFilterModel filter = null, string field = Constants.DEFAULTEDITIONSORT, bool orderAsc = false, int page = Constants.DEFAULTPAGE, bool getRemoved = false, int pageSize = Constants.PRINTINGEDITIONPAGESIZE);
        public PrintingEditionResponseModel GetPrintingEditions(int page = Constants.DEFAULTPAGE);
        public List<PrintingEditionEntity> GetPrintingEditionsRange(PrintingEditionFilterModel printingEditionFilter = null, bool getRemoved = false);
        public PrintingEditionModel GetPrintingEdition(int id);
        public PrintingEditionModel GetPrintingEditionByTitle(string title);
    }
}
