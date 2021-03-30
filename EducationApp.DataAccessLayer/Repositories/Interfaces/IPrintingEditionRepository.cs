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
        public List<PrintingEditionEntity> Get(Expression<Func<PrintingEditionEntity, bool>> filter = null,
                Func<IQueryable<PrintingEditionEntity>, IOrderedQueryable<PrintingEditionEntity>> orderBy = null,
                bool getRemoved = false,
                int page = Constants.DEFAULTPAGE, int pageSize = Constants.PRINTINGEDITIONPAGESIZE);
        public List<PrintingEditionEntity> GetAll(Expression<Func<PrintingEditionEntity, bool>> filter = null,
            bool getRemoved = false);
        public void Update(PrintingEditionEntity printingEdition, AuthorEntity author = null);
    }
}
