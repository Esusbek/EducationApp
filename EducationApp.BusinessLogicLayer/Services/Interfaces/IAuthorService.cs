using EducationApp.BusinessLogicLayer.Models.Authors;
using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EducationApp.BusinessLogicLayer.Services.Interfaces
{
    public interface IAuthorService
    {
        public void AddAuthor(AuthorModel author);
        public void UpdateAuthor(AuthorModel author);
        public void AddPrintingEditionToAuthor(AuthorModel author, PrintingEditionModel printingEdition);
        public void DeleteAuthor(AuthorModel author);
        public List<AuthorModel> GetAuthorsFiltered(AuthorFilterModel authorFilter = null,
            Func<IQueryable<AuthorEntity>, IOrderedQueryable<AuthorEntity>> orderBy = null,
            int page = Constants.DEFAULTPAGE, bool getRemoved = false);
        public List<AuthorModel> GetAuthors(int page = Constants.DEFAULTPAGE);
        public AuthorModel GetAuthor(int id);
        public AuthorModel GetAuthorByName(string name);
    }
}
