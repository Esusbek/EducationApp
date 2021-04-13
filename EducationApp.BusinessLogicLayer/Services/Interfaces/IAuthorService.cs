using EducationApp.BusinessLogicLayer.Models.Authors;
using EducationApp.DataAccessLayer.FilterModels;
using EducationApp.Shared.Constants;
using System.Collections.Generic;

namespace EducationApp.BusinessLogicLayer.Services.Interfaces
{
    public interface IAuthorService
    {
        public void AddAuthor(AuthorModel author);
        public void UpdateAuthor(AuthorModel author);
        public void DeleteAuthor(AuthorModel author);
        public List<AuthorModel> GetAuthorsFiltered(AuthorFilterModel authorFilter = null, string field = null, bool ascending = true, int page = Constants.DEFAULTPAGE, bool getRemoved = false);
        public int GetLastPage(AuthorFilterModel authorFilter = null, bool getRemoved = false);
        public List<AuthorModel> GetAuthors(int page = Constants.DEFAULTPAGE);
        public List<AuthorModel> GetAllAuthors();
        public AuthorModel GetAuthor(int id);
        public AuthorModel GetAuthorByName(string name);
    }
}
