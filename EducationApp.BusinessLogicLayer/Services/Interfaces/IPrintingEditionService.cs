using EducationApp.BusinessLogicLayer.Models.Authors;
using EducationApp.BusinessLogicLayer.Models.Helpes;
using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EducationApp.BusinessLogicLayer.Services.Interfaces
{
    public interface IPrintingEditionService
    {
        public void AddPrintingEdition(PrintingEditionModel printingEdition);
        public void UpdatePrintingEdition(PrintingEditionModel printingEdition);
        public void AddAuthorToPrintingEdition(PrintingEditionModel printingEdition, AuthorModel author);
        public void DeletePrintingEdition(PrintingEditionModel printingEdition);
        public PrintingEditionsInfo GetInfo();
        public int GetLastPage(PrintingEditionFilterModel printingEditionFilter);
        public List<PrintingEditionModel> GetPrintingEditionsFiltered(PrintingEditionFilterModel filter = null,
            Func<IQueryable<PrintingEditionEntity>, IOrderedQueryable<PrintingEditionEntity>> orderBy = null,
            int page = Constants.DEFAULTPAGE, bool getRemoved = false);
        public List<PrintingEditionModel> GetPrintingEditions(int page = Constants.DEFAULTPAGE);
        public List<PrintingEditionEntity> GetPrintingEditionsRange(Expression<Func<PrintingEditionEntity, bool>> filter = null,
            Func<IQueryable<PrintingEditionEntity>, IOrderedQueryable<PrintingEditionEntity>> orderBy = null,
            int page = Constants.DEFAULTPAGE, bool getRemoved = false);
        public PrintingEditionModel GetPrintingEdition(int id);
        public PrintingEditionModel GetPrintingEditionByTitle(string title);
    }
}
