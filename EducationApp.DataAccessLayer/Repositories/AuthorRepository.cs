using EducationApp.DataAccessLayer.Entities;
using EducationApp.Shared.Constants;
using Microsoft.EntityFrameworkCore;
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
        public List<AuthorEntity> Get(Expression<Func<AuthorEntity, bool>> filter = null,
            Func<IQueryable<AuthorEntity>, IOrderedQueryable<AuthorEntity>> orderBy = null,
            bool getRemoved = false,
            int page = Constants.DEFAULTPAGE)
        {
            return base.Get(filter, orderBy, getRemoved)
                .Skip((page - Constants.DEFAULTPREVIOUSPAGEOFFSET) * Constants.AUTHORPAGESIZE)
                .Take(Constants.AUTHORPAGESIZE).ToList();
        }
        public List<AuthorEntity> GetAll(Expression<Func<AuthorEntity, bool>> filter = null,
            bool getRemoved = false)
        {
            return base.Get(filter,getRemoved: getRemoved);
        }

        public void Update(AuthorEntity author, PrintingEditionEntity printingEdition=null)
        {
            if (printingEdition is not null && author.PrintingEditions.Contains(printingEdition))
            {
                author.PrintingEditions.Add(printingEdition);
            }
            base.Update(author);
        }
        
    }
}
