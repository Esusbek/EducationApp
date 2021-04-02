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
            string field = null, bool ascending = true,
            bool getRemoved = false,
            int page = Constants.DEFAULTPAGE, int pageSize = Constants.PRINTINGEDITIONPAGESIZE)
        {
            return base.Get(filter, field, ascending, getRemoved)
                .Skip((page - Constants.DEFAULTPREVIOUSPAGEOFFSET) * pageSize)
                .Take(pageSize).ToList();
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
