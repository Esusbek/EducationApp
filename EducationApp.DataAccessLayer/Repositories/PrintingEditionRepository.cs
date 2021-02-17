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
        public IEnumerable<PrintingEditionEntity> GetAll(int page = Constants.Defaults.DefaultPage)
        {
            return base.GetAll()
                .Skip((page - 1) * Constants.Pages.PrintingEditionPageSize)
                .Take(Constants.Pages.PrintingEditionPageSize).ToList();
        }
        public IEnumerable<PrintingEditionEntity> Get(Expression<Func<PrintingEditionEntity, bool>> filter = null,
            Func<IQueryable<PrintingEditionEntity>, IOrderedQueryable<PrintingEditionEntity>> orderBy = null,
            bool getRemoved = false,
            int page = Constants.Defaults.DefaultPage)
        {
            return base.Get(filter, orderBy, getRemoved)
                .Skip((page - 1) * Constants.Pages.PrintingEditionPageSize)
                .Take(Constants.Pages.PrintingEditionPageSize).ToList();
        }
        public void AddAuthorToPrintingEdition(PrintingEditionEntity printingEdition, AuthorEntity author)
        {
            if (printingEdition.Authors.Contains(author))
            {
                return;
            }
            printingEdition.Authors.Add(author);
            base.Update(printingEdition);
        }
    }
}
