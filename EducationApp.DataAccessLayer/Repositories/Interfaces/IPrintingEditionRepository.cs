using EducationApp.DataAccessLayer.Entities;
using EducationApp.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EducationApp.DataAccessLayer.Repositories.Interfaces
{
    public interface IPrintingEditionRepository : Base.BaseInterface.IBaseRepository<PrintingEditionEntity>
    {
        public IEnumerable<PrintingEditionEntity> GetAll(int page);
        public IEnumerable<PrintingEditionEntity> Get(Expression<Func<PrintingEditionEntity, bool>> filter = null,
                Func<IQueryable<PrintingEditionEntity>, IOrderedQueryable<PrintingEditionEntity>> orderBy = null,
                bool getRemoved = false,
                int page = Constants.DEFAULTPAGE);
        public IEnumerable<PrintingEditionEntity> GetNoPagination(Expression<Func<PrintingEditionEntity, bool>> filter = null,
            Func<IQueryable<PrintingEditionEntity>, IOrderedQueryable<PrintingEditionEntity>> orderBy = null,
            bool getRemoved = false);
        public void AddAuthorToPrintingEdition(PrintingEditionEntity printingEdition, AuthorEntity author);
    }
}
