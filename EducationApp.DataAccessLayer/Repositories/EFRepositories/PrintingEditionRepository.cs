using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.FilterModels;
using EducationApp.DataAccessLayer.Repositories.Base;
using EducationApp.DataAccessLayer.Repositories.Interfaces;
using EducationApp.Shared.Constants;
using EducationApp.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EducationApp.DataAccessLayer.Repositories.EFRepositories
{
    public class PrintingEditionRepository : BaseRepository<PrintingEditionEntity>, IPrintingEditionRepository
    {
        public PrintingEditionRepository(ApplicationContext dbContext)
            : base(dbContext)
        {
        }
        public List<PrintingEditionEntity> Get(PrintingEditionFilterModel printingEditionFilter = null, string field = null, bool ascending = true, bool getRemoved = false, int page = Constants.DEFAULTPAGE, int pageSize = Constants.PRINTINGEDITIONPAGESIZE)
        {
            page = page < Constants.DEFAULTPAGE ? Constants.DEFAULTPAGE : page;
            Expression<Func<PrintingEditionEntity, bool>> filter = null;
            if (printingEditionFilter is not null)
            {
                filter = edition => ((printingEditionFilter.IsBook && edition.Type == Enums.PrintingEditionType.Book)
                || (printingEditionFilter.IsNewspaper && edition.Type == Enums.PrintingEditionType.Newspaper)
                || (printingEditionFilter.IsJournal && edition.Type == Enums.PrintingEditionType.Journal)) &&
                (string.IsNullOrWhiteSpace(printingEditionFilter.Title) || edition.Title.Contains(printingEditionFilter.Title)) &&
                (printingEditionFilter.LowPrice == default || edition.Price >= printingEditionFilter.LowPrice) &&
                (printingEditionFilter.HighPrice == default || edition.Price <= printingEditionFilter.HighPrice) &&
                (!printingEditionFilter.Type.Any() || printingEditionFilter.Type.Contains(edition.Type)) &&
                (!printingEditionFilter.EditionIds.Any() || printingEditionFilter.EditionIds.Contains(edition.Id));
            }
            return base.Get(filter, field, ascending, getRemoved)
                .Skip((page - Constants.DEFAULTPREVIOUSPAGEOFFSET) * pageSize)
                .Take(pageSize).ToList();
        }
        public PrintingEditionEntity GetOne(PrintingEditionFilterModel printingEditionFilter = null, string field = null, bool ascending = true, bool getRemoved = false)
        {
            Expression<Func<PrintingEditionEntity, bool>> filter = null;
            if (printingEditionFilter is not null)
            {
                filter = edition => ((printingEditionFilter.IsBook && edition.Type == Enums.PrintingEditionType.Book)
                || (printingEditionFilter.IsNewspaper && edition.Type == Enums.PrintingEditionType.Newspaper)
                || (printingEditionFilter.IsJournal && edition.Type == Enums.PrintingEditionType.Journal)) &&
                (string.IsNullOrWhiteSpace(printingEditionFilter.Title) || edition.Title.Contains(printingEditionFilter.Title)) &&
                (printingEditionFilter.LowPrice == default || edition.Price >= printingEditionFilter.LowPrice) &&
                (printingEditionFilter.HighPrice == default || edition.Price <= printingEditionFilter.HighPrice) &&
                (!printingEditionFilter.Type.Any() || printingEditionFilter.Type.Contains(edition.Type)) &&
                (!printingEditionFilter.EditionIds.Any() || printingEditionFilter.EditionIds.Contains(edition.Id));
            }
            return base.GetOne(filter, field, ascending, getRemoved);
        }
        public List<PrintingEditionEntity> GetAll(PrintingEditionFilterModel printingEditionFilter = null, bool getRemoved = false)
        {
            Expression<Func<PrintingEditionEntity, bool>> filter = null;
            if (printingEditionFilter is not null)
            {
                filter = edition => ((printingEditionFilter.IsBook && edition.Type == Enums.PrintingEditionType.Book)
                || (printingEditionFilter.IsNewspaper && edition.Type == Enums.PrintingEditionType.Newspaper)
                || (printingEditionFilter.IsJournal && edition.Type == Enums.PrintingEditionType.Journal)) &&
                (string.IsNullOrWhiteSpace(printingEditionFilter.Title) || edition.Title.Contains(printingEditionFilter.Title)) &&
                (printingEditionFilter.LowPrice == default || edition.Price >= printingEditionFilter.LowPrice) &&
                (printingEditionFilter.HighPrice == default || edition.Price <= printingEditionFilter.HighPrice) &&
                (!printingEditionFilter.Type.Any() || printingEditionFilter.Type.Contains(edition.Type)) &&
                (!printingEditionFilter.EditionIds.Any() || printingEditionFilter.EditionIds.Contains(edition.Id));
            }
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
        public override void Delete(PrintingEditionEntity entityToDelete)
        {
            entityToDelete.Authors.Clear();
            base.Delete(entityToDelete);
        }
    }
}
