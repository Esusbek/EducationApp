using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.FilterModels;
using EducationApp.Shared.Constants;
using System.Collections.Generic;

namespace EducationApp.DataAccessLayer.Repositories.Interfaces
{
    public interface IAuthorRepository
    {
        public List<AuthorEntity> Get(AuthorFilterModel authorFilter = null, string field = null, bool ascending = true, bool getRemoved = false, int page = Constants.DEFAULTPAGE);
        public List<AuthorEntity> GetAll(AuthorFilterModel authorFilter = null, bool getRemoved = false);
        public void Update(AuthorEntity author, PrintingEditionEntity printingEdition = null);
        public AuthorEntity GetById(int id);
        public List<AuthorEntity> GetAll();
        public void Insert(AuthorEntity entity);
        public void InsertRange(IEnumerable<AuthorEntity> entity);
        public void Update(AuthorEntity entity);
        public void Delete(AuthorEntity entityToDelete);
        public void SaveChanges();
    }
}
