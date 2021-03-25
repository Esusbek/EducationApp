using EducationApp.DataAccessLayer.Entities;
using EducationApp.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EducationApp.DataAccessLayer.Repositories
{
    public class PrintingEditionRepository : Base.BaseRepository<PrintingEditionEntity>, Interfaces.IPrintingEditionRepository
    {
        public PrintingEditionRepository(AppContext.ApplicationContext dbContext)
            : base(dbContext)
        {
        }
        public List<PrintingEditionEntity> Get(Expression<Func<PrintingEditionEntity, bool>> filter = null,
            Func<IQueryable<PrintingEditionEntity>, IOrderedQueryable<PrintingEditionEntity>> orderBy = null,
            bool getRemoved = false,
            int page = Constants.DEFAULTPAGE)
        {
            return base.Get(filter, orderBy, getRemoved)
                .Skip((page - Constants.DEFAULTPREVIOUSPAGEOFFSET) * Constants.PRINTINGEDITIONPAGESIZE)
                .Take(Constants.PRINTINGEDITIONPAGESIZE).ToList();
        }
        public List<PrintingEditionEntity> GetAll(Expression<Func<PrintingEditionEntity, bool>> filter = null,
            bool getRemoved = false)
        {
            return base.Get(filter, getRemoved: getRemoved);
        }
        public void Update(PrintingEditionEntity printingEdition, AuthorEntity author = null)
        {
            if (author is not null && !printingEdition.Authors.Contains(author))
            {
                printingEdition.Authors.Add(author);
            }
            base.Update(printingEdition);
        }
    }
}
