using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repositories.Base.BaseInterface;
using EducationApp.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EducationApp.DataAccessLayer.Repositories.Interfaces
{
    public interface IAuthorRepository : IBaseRepository<AuthorEntity>
    {
        public List<AuthorEntity> Get(Expression<Func<AuthorEntity, bool>> filter = null,
                string field = null, bool ascending = true,
                bool getRemoved = false,
                int page = Constants.DEFAULTPAGE);
        public List<AuthorEntity> GetAll(Expression<Func<AuthorEntity, bool>> filter = null,
            bool getRemoved = false);
        public void Update(AuthorEntity author, PrintingEditionEntity printingEdition = null);
    }
}
