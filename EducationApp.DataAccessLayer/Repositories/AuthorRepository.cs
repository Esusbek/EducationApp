using EducationApp.DataAccessLayer.Entities;
using EducationApp.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EducationApp.DataAccessLayer.Repositories
{
    public class AuthorRepository : Base.BaseRepository<AuthorEntity>, Interfaces.IAuthorRepository
    {
        public AuthorRepository(AppContext.ApplicationContext dbContext)
            : base(dbContext)
        {
        }
        public IEnumerable<AuthorEntity> GetAll(int page = Constants.DEFAULTPAGE)
        {
            return base.GetAll()
                .Skip((page - Constants.DEFAULTPREVIOUSPAGEOFFSET) * Constants.AUTHORPAGESIZE)
                .Take(Constants.AUTHORPAGESIZE);
        }
        public IEnumerable<AuthorEntity> Get(Expression<Func<AuthorEntity, bool>> filter = null,
            Func<IQueryable<AuthorEntity>, IOrderedQueryable<AuthorEntity>> orderBy = null,
            bool getRemoved = false,
            int page = Constants.DEFAULTPAGE)
        {
            return base.Get(filter, orderBy, getRemoved)
                .Skip((page - Constants.DEFAULTPREVIOUSPAGEOFFSET) * Constants.AUTHORPAGESIZE)
                .Take(Constants.AUTHORPAGESIZE);
        }

        public void AddPrintingEditionToAuthor(AuthorEntity author, PrintingEditionEntity printingEdition)
        {
            if (author.PrintingEditions.Contains(printingEdition))
            {
                return;
            }
            author.PrintingEditions.Add(printingEdition);
            base.Update(author);
        }
    }
}
