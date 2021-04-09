using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repositories.Base;
using EducationApp.DataAccessLayer.Repositories.Interfaces;
using EducationApp.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EducationApp.DataAccessLayer.Repositories.EFRepositories
{
    public class AuthorRepository : BaseRepository<AuthorEntity>, IAuthorRepository
    {
        public AuthorRepository(ApplicationContext dbContext)
            : base(dbContext)
        {
        }
        public List<AuthorEntity> Get(Expression<Func<AuthorEntity, bool>> filter = null, string field = null, bool ascending = true, bool getRemoved = false, int page = Constants.DEFAULTPAGE)
        {
            return base.Get(filter, field, ascending, getRemoved)
                .Skip((page - Constants.DEFAULTPREVIOUSPAGEOFFSET) * Constants.AUTHORPAGESIZE)
                .Take(Constants.AUTHORPAGESIZE).ToList();
        }
        public List<AuthorEntity> GetAll(Expression<Func<AuthorEntity, bool>> filter = null, bool getRemoved = false)
        {
            return base.Get(filter, getRemoved: getRemoved);
        }

        public void Update(AuthorEntity author, PrintingEditionEntity printingEdition = null)
        {
            if (printingEdition is not null && author.PrintingEditions.Contains(printingEdition))
            {
                author.PrintingEditions.Add(printingEdition);
            }
            base.Update(author);
        }

    }
}
