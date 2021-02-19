using AutoMapper;
using EducationApp.BusinessLogicLayer.Models.Authors;
using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repositories.Interfaces;
using EducationApp.Shared.Constants;
using EducationApp.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;

namespace EducationApp.BusinessLogicLayer.Services
{
    public class PrintingEditionService : Interfaces.IPrintingEditionService
    {
        private readonly IPrintingEditionRepository _printingEditionRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        public PrintingEditionService(IMapper mapper, IPrintingEditionRepository printingEditionRepository,
            IAuthorRepository authorRepository)
        {
            _printingEditionRepository = printingEditionRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
        }
        public void AddPrintingEdition(PrintingEditionModel printingEdition)
        {
            var dbPrintingEdition = _mapper.Map<PrintingEditionEntity>(printingEdition);
            _printingEditionRepository.Insert(dbPrintingEdition);
        }
        public void UpdatePrintingEdition(PrintingEditionModel printingEdition)
        {
            var dbPrintingEdition = _printingEditionRepository.GetById(printingEdition.Id);
            if (dbPrintingEdition is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.PRINTINGEDITIONNOTFOUNDERROR);
            }
            var updatedPrintingEdition = _mapper.Map<PrintingEditionEntity>(printingEdition);
            updatedPrintingEdition.CreatedAt = dbPrintingEdition.CreatedAt;
            _printingEditionRepository.Update(updatedPrintingEdition);
        }
        public void AddAuthorToPrintingEdition(PrintingEditionModel printingEdition, AuthorModel author)
        {
            var dbPrintingEdition = _printingEditionRepository.GetById(printingEdition.Id);
            if (dbPrintingEdition is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.PRINTINGEDITIONNOTFOUNDERROR);
            }
            var dbAuthor = _authorRepository.GetById(author.Id);
            if (dbAuthor is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.AUTHORNOTFOUNDERROR);
            }
            dbPrintingEdition.Authors = new List<AuthorEntity>();
            _printingEditionRepository.AddAuthorToPrintingEdition(dbPrintingEdition, dbAuthor);
        }
        public void DeletePrintingEdition(PrintingEditionModel printingEdition)
        {
            var dbPrintingEdition = _printingEditionRepository.GetById(printingEdition.Id);
            _printingEditionRepository.Delete(dbPrintingEdition);
        }
        public List<PrintingEditionModel> GetPrintingEditionsFiltered(PrintingEditionFilterModel printingEditionFilter = null,
            Func<IQueryable<PrintingEditionEntity>, IOrderedQueryable<PrintingEditionEntity>> orderBy = null,
            int page = Constants.DEFAULTPAGE, bool getRemoved = false)
        {
            Expression<Func<PrintingEditionEntity, bool>> filter = null;
            if (printingEditionFilter is not null)
            {
                filter = edition => (string.IsNullOrWhiteSpace(printingEditionFilter.Title) || edition.Title.Contains(printingEditionFilter.Title)) &&
                (printingEditionFilter.LowPrice == 0 || edition.Price > printingEditionFilter.LowPrice) &&
                (printingEditionFilter.HighPrice == 0 || edition.Price < printingEditionFilter.HighPrice) &&
                (printingEditionFilter.Type == default || edition.Type == printingEditionFilter.Type);
            }
            var dbPrintingEditions = _printingEditionRepository.Get(filter, orderBy, getRemoved, page);
            var printingEditions = new List<PrintingEditionModel>();
            foreach (var printingEdition in dbPrintingEditions)
            {
                printingEditions.Add(_mapper.Map<PrintingEditionModel>(printingEdition));
            }
            return printingEditions;
        }
        public List<PrintingEditionModel> GetPrintingEditions(int page = Constants.DEFAULTPAGE)
        {
            var dbPrintingEditions = _printingEditionRepository.Get(page: page);
            var printingEditions = new List<PrintingEditionModel>();
            foreach (var printingEdition in dbPrintingEditions)
            {
                printingEditions.Add(_mapper.Map<PrintingEditionModel>(printingEdition));
            }
            return printingEditions;
        }
        public List<PrintingEditionEntity> GetPrintingEditionsRange(Expression<Func<PrintingEditionEntity, bool>> filter = null,
            Func<IQueryable<PrintingEditionEntity>, IOrderedQueryable<PrintingEditionEntity>> orderBy = null,
            int page = Constants.DEFAULTPAGE, bool getRemoved = false)
        {
            var dbPrintingEditions = _printingEditionRepository.GetNoPagination(filter, orderBy, getRemoved);
            return dbPrintingEditions.ToList();
        }
        public PrintingEditionModel GetPrintingEdition(int id)
        {
            return _mapper.Map<PrintingEditionModel>(_printingEditionRepository.GetById(id));
        }
        public PrintingEditionModel GetPrintingEditionByTitle(string title)
        {
            return _mapper.Map<PrintingEditionModel>(_printingEditionRepository.Get(edition => edition.Title == title).First());
        }
    }
}
