using EducationApp.DataAccessLayer.Entities;
using EducationApp.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EducationApp.DataAccessLayer.Repositories.Interfaces
{
    public interface IAuthorRepository : Base.BaseInterface.IBaseRepository<AuthorEntity>
    {
        public IEnumerable<AuthorEntity> GetAll(int page);
        public IEnumerable<AuthorEntity> Get(Expression<Func<AuthorEntity, bool>> filter = null,
                Func<IQueryable<AuthorEntity>, IOrderedQueryable<AuthorEntity>> orderBy = null,
                bool getRemoved = false,
                int page = Constants.Pages.InitialPage);
        public void AddPrintingEditionToAuthor(AuthorEntity author, PrintingEditionEntity printingEdition);
    }
}
