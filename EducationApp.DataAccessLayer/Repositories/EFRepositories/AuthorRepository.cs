using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.FilterModels;
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
        public List<AuthorEntity> Get(AuthorFilterModel authorFilter = null, string field = null, bool ascending = true, bool getRemoved = false, int page = Constants.DEFAULTPAGE)
        {
            page = page < Constants.DEFAULTPAGE ? Constants.DEFAULTPAGE : page;
            Expression<Func<AuthorEntity, bool>> filter = null;
            if (authorFilter is not null)
            {
                filter = author => (string.IsNullOrWhiteSpace(authorFilter.Name) || author.Name.Contains(authorFilter.Name)) &&
                (!authorFilter.EditionAuthors.Any() || authorFilter.EditionAuthors.Contains(author.Name));
            }
            
            return base.Get(filter, field, ascending, getRemoved)
                .Skip((page - Constants.DEFAULTPREVIOUSPAGEOFFSET) * Constants.AUTHORPAGESIZE)
                .Take(Constants.AUTHORPAGESIZE).ToList();
        }
        public List<AuthorEntity> GetAll(AuthorFilterModel authorFilter = null, bool getRemoved = false)
        {
            Expression<Func<AuthorEntity, bool>> filter = null;
            if (authorFilter is not null)
            {
                filter = author => (string.IsNullOrWhiteSpace(authorFilter.Name) || author.Name.Contains(authorFilter.Name)) &&
                (!authorFilter.EditionAuthors.Any() || authorFilter.EditionAuthors.Contains(author.Name));
            }
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
