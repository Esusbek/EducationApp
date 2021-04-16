using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.FilterModels;
using EducationApp.Shared.Constants;
using System.Collections.Generic;

namespace EducationApp.DataAccessLayer.Repositories.Interfaces
{
    public interface IPrintingEditionRepository
    {
        public List<PrintingEditionEntity> Get(PrintingEditionFilterModel printingEditionFilter = null, string field = null, bool ascending = true, bool getRemoved = false, int page = Constants.DEFAULTPAGE, int pageSize = Constants.PRINTINGEDITIONPAGESIZE);
        public PrintingEditionEntity GetOne(PrintingEditionFilterModel printingEditionFilter = null, string field = null, bool ascending = true, bool getRemoved = false);
        public List<PrintingEditionEntity> GetAll(PrintingEditionFilterModel printingEditionFilter = null, bool getRemoved = false);
        public void Update(PrintingEditionEntity printingEdition, AuthorEntity author = null);
        public PrintingEditionEntity GetById(int id);
        public void Insert(PrintingEditionEntity entity);
        public void InsertRange(IEnumerable<PrintingEditionEntity> entity);
        public void Delete(PrintingEditionEntity entityToDelete);
        public void SaveChanges();
    }
}
