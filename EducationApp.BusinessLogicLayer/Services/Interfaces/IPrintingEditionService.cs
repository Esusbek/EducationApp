﻿using EducationApp.BusinessLogicLayer.Models.Authors;
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
        public List<PrintingEditionModel> GetPrintingEditionsAdmin(bool getBook = true, bool getNewspaper = true, bool getJournal = true,
            string field = Constants.DEFAULTEDITIONSORT, bool orderAsc = false, int page = Constants.DEFAULTPAGE,
            bool getRemoved = false);
        public PrintingEditionsInfoModel GetInfo(PrintingEditionFilterModel printingEditionFilter = null, int pageSize = Constants.PRINTINGEDITIONPAGESIZE);
        public int GetLastPage(bool getBook = true, bool getNewspaper = true, bool getJournal = true);
        public PrintingEditionResponseModel GetPrintingEditionsFiltered(PrintingEditionFilterModel filter = null,
            string field = "Price", bool orderAsc = false, int page = Constants.DEFAULTPAGE, bool getRemoved = false, int pageSize = Constants.PRINTINGEDITIONPAGESIZE);
        public PrintingEditionResponseModel GetPrintingEditions(int page = Constants.DEFAULTPAGE);
        public List<PrintingEditionEntity> GetPrintingEditionsRange(Expression<Func<PrintingEditionEntity, bool>> filter = null, bool getRemoved = false);
        public PrintingEditionModel GetPrintingEdition(int id);
        public PrintingEditionModel GetPrintingEditionByTitle(string title);
    }
}
