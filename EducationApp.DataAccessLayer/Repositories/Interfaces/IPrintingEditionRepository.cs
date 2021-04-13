using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.FilterModels;
using EducationApp.DataAccessLayer.Repositories.Base.BaseInterface;
using EducationApp.Shared.Constants;
using System.Collections.Generic;

namespace EducationApp.DataAccessLayer.Repositories.Interfaces
{
    public interface IPrintingEditionRepository : IBaseRepository<PrintingEditionEntity>
    {
        public List<PrintingEditionEntity> Get(PrintingEditionFilterModel printingEditionFilter = null,string field = null, bool ascending = true,bool getRemoved = false,int page = Constants.DEFAULTPAGE, int pageSize = Constants.PRINTINGEDITIONPAGESIZE);
        public List<PrintingEditionEntity> GetAll(PrintingEditionFilterModel printingEditionFilter = null,bool getRemoved = false);
        public void Update(PrintingEditionEntity printingEdition, AuthorEntity author = null);
    }
}
