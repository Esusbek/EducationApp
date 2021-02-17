using EducationApp.BusinessLogicLayer.Models.Authors;
using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using EducationApp.Shared.Constants;
using System.Collections.Generic;

namespace EducationApp.BusinessLogicLayer.Services.Interfaces
{
    public interface IAuthorService
    {
        public void AddAuthor(AuthorModel author);
        public void UpdateAuthor(AuthorModel author);
        public void AddPrintingEditionToAuthor(AuthorModel author, PrintingEditionModel printingEdition);
        public void DeleteAuthor(AuthorModel author);
        public List<AuthorModel> GetAuthorsFiltered(AuthorFilterModel authorFilter = null,
            int page = Constants.Defaults.DefaultPage, bool getRemoved = false);
        public List<AuthorModel> GetAuthors(int page = Constants.Defaults.DefaultPage);
        public AuthorModel GetAuthor(int id);
        public AuthorModel GetAuthor(string name);
    }
}
