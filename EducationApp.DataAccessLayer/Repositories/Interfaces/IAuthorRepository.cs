using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.FilterModels;
using EducationApp.DataAccessLayer.Repositories.Base.BaseInterface;
using EducationApp.Shared.Constants;
using System.Collections.Generic;

namespace EducationApp.DataAccessLayer.Repositories.Interfaces
{
    public interface IAuthorRepository: IBaseRepository<AuthorEntity>
    {
        public List<AuthorEntity> Get(AuthorFilterModel authorFilter = null, string field = null, bool ascending = true, bool getRemoved = false, int page = Constants.DEFAULTPAGE);
        public List<AuthorEntity> GetAll(AuthorFilterModel authorFilter = null, bool getRemoved = false);
        public int GetCount(AuthorFilterModel authorFilter = null, bool getRemoved = false);
        public void Update(AuthorEntity author, PrintingEditionEntity printingEdition = null);
    }
}
