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
        public IQueryable<AuthorEntity> GetAll(int page);
        public IQueryable<AuthorEntity> Get(Expression<Func<AuthorEntity, bool>> filter = null,
                Func<IQueryable<AuthorEntity>, IOrderedQueryable<AuthorEntity>> orderBy = null,
                bool getRemoved = false,
                int page = Constants.DEFAULTPAGE);
        public IQueryable<AuthorEntity> GetNoPagination(Expression<Func<AuthorEntity, bool>> filter = null,
            Func<IQueryable<AuthorEntity>, IOrderedQueryable<AuthorEntity>> orderBy = null,
            bool getRemoved = false);
        public void AddPrintingEditionToAuthor(AuthorEntity author, PrintingEditionEntity printingEdition);
    }
}
