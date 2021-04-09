using AutoMapper;
using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using EducationApp.BusinessLogicLayer.Providers.Interfaces;
using EducationApp.BusinessLogicLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repositories.Interfaces;
using EducationApp.Shared.Constants;
using EducationApp.Shared.Enums;
using EducationApp.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;

namespace EducationApp.BusinessLogicLayer.Services
{
    public class PrintingEditionService : IPrintingEditionService
    {
        private readonly IPrintingEditionRepository _printingEditionRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly IValidationProvider _validator;
        public PrintingEditionService(IMapper mapper, IPrintingEditionRepository printingEditionRepository,
            IAuthorRepository authorRepository, IValidationProvider validationProvider)
        {
            _printingEditionRepository = printingEditionRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
            _validator = validationProvider;
        }
        public void AddPrintingEdition(PrintingEditionModel printingEdition)
        {
            _validator.ValidatePrintingEdition(printingEdition);
            var dbPrintingEdition = _mapper.Map<PrintingEditionEntity>(printingEdition);
            dbPrintingEdition.Status = Enums.PrintingEditionStatusType.InStock;
            var authors = _authorRepository.GetAll(author => printingEdition.Authors.Contains(author.Name));
            dbPrintingEdition.Authors = new List<AuthorEntity>(authors);
            _printingEditionRepository.Insert(dbPrintingEdition);
        }
        public void UpdatePrintingEdition(PrintingEditionModel printingEdition)
        {
            _validator.ValidatePrintingEdition(printingEdition);
            var dbPrintingEdition = _printingEditionRepository.GetById(printingEdition.Id);
            if (dbPrintingEdition is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.PRINTINGEDITIONNOTFOUNDERROR);
            }
            _mapper.Map(printingEdition, dbPrintingEdition);
            var authors = _authorRepository.GetAll(author => printingEdition.Authors.Contains(author.Name));
            dbPrintingEdition.Authors.Clear();
            var dbAuthors = dbPrintingEdition.Authors.ToList();
            dbAuthors.AddRange(authors);
            dbPrintingEdition.Authors = dbAuthors;
            _printingEditionRepository.Update(dbPrintingEdition);
        }

        public void DeletePrintingEdition(PrintingEditionModel printingEdition)
        {
            var dbPrintingEdition = _printingEditionRepository.GetById(printingEdition.Id);
            if (dbPrintingEdition is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.PRINTINGEDITIONNOTFOUNDERROR);
            }
            _printingEditionRepository.Delete(dbPrintingEdition);

        }
        public List<PrintingEditionModel> GetPrintingEditionsAdmin(bool getBook = true, bool getNewspaper = true, bool getJournal = true,
            string field = Constants.DEFAULTEDITIONSORT, bool orderAsc = false, int page = Constants.DEFAULTPAGE,
            bool getRemoved = false)
        {
            Expression<Func<PrintingEditionEntity, bool>> filter = edition => (getBook && edition.Type == Enums.PrintingEditionType.Book)
            || (getNewspaper && edition.Type == Enums.PrintingEditionType.Newspaper)
            || (getJournal && edition.Type == Enums.PrintingEditionType.Journal);
            var dbPrintingEditions = _printingEditionRepository.Get(filter, field, orderAsc, getRemoved, page, Constants.ADMINPRINTINGEDITIONPAGESIZE).ToList();
            var printingEditions = new List<PrintingEditionModel>();
            foreach (var printingEdition in dbPrintingEditions)
            {
                var mappedEdition = _mapper.Map<PrintingEditionModel>(printingEdition);
                mappedEdition.Authors = printingEdition.Authors.Select(author => author.Name).ToList();
                printingEditions.Add(mappedEdition);
            }
            return printingEditions;
        }
        public PrintingEditionResponseModel GetPrintingEditionsFiltered(PrintingEditionFilterModel printingEditionFilter = null,
            string field = Constants.DEFAULTEDITIONSORT, bool orderAsc = false, int page = Constants.DEFAULTPAGE,
            bool getRemoved = false, int pageSize = Constants.PRINTINGEDITIONPAGESIZE)
        {
            Expression<Func<PrintingEditionEntity, bool>> filter = null;

            if (printingEditionFilter is not null)
            {
                filter = edition => (string.IsNullOrWhiteSpace(printingEditionFilter.Title) || edition.Title.Contains(printingEditionFilter.Title)) &&
                (printingEditionFilter.LowPrice == default || edition.Price >= printingEditionFilter.LowPrice) &&
                (printingEditionFilter.HighPrice == default || edition.Price <= printingEditionFilter.HighPrice) &&
                (printingEditionFilter.Type == default || printingEditionFilter.Type.Contains(edition.Type));
            }

            var dbPrintingEditions = _printingEditionRepository.Get(filter, field, orderAsc, getRemoved, page, pageSize).ToList();
            var printingEditions = new List<PrintingEditionModel>();
            foreach (var printingEdition in dbPrintingEditions)
            {
                var mappedEdition = _mapper.Map<PrintingEditionModel>(printingEdition);
                mappedEdition.Authors = printingEdition.Authors.Select(author => author.Name).ToList();
                printingEditions.Add(mappedEdition);

            }
            var info = GetInfo(printingEditionFilter);
            return new PrintingEditionResponseModel
            {
                Books = printingEditions,
                Info = info
            };
        }
        private PrintingEditionsInfoModel GetInfo(PrintingEditionFilterModel printingEditionFilter = null, int pageSize = Constants.PRINTINGEDITIONPAGESIZE)
        {
            Expression<Func<PrintingEditionEntity, bool>> filter = null;
            if (printingEditionFilter is not null)
            {
                filter = edition => (string.IsNullOrWhiteSpace(printingEditionFilter.Title) || edition.Title.Contains(printingEditionFilter.Title)) &&
                (printingEditionFilter.LowPrice == default || edition.Price >= printingEditionFilter.LowPrice) &&
                (printingEditionFilter.HighPrice == default || edition.Price <= printingEditionFilter.HighPrice) &&
                (printingEditionFilter.Type == default || printingEditionFilter.Type.Contains(edition.Type));
            }
            var dbPrintingEditions = _printingEditionRepository.GetAll(filter).ToList();
            var lastPage = (int)Math.Ceiling(dbPrintingEditions.Count / (double)pageSize);
            if (printingEditionFilter is not null)
            {
                filter = edition => (string.IsNullOrWhiteSpace(printingEditionFilter.Title) || edition.Title.Contains(printingEditionFilter.Title)) &&
                (printingEditionFilter.Type == default || printingEditionFilter.Type.Contains(edition.Type));
            }
            dbPrintingEditions = _printingEditionRepository.GetAll(filter).ToList();
            var min = dbPrintingEditions.Aggregate((currentMin, x) => (currentMin == null || x.Price < currentMin.Price ? x : currentMin)).Price;
            var max = dbPrintingEditions.Aggregate((currentMax, x) => (currentMax == null || x.Price > currentMax.Price ? x : currentMax)).Price;

            return new PrintingEditionsInfoModel
            {
                MaxPrice = max,
                MinPrice = min,
                LastPage = lastPage
            };
        }
        public int GetLastPage(bool getBook = true, bool getNewspaper = true, bool getJournal = true)
        {
            Expression<Func<PrintingEditionEntity, bool>> filter = edition => (getBook && edition.Type == Enums.PrintingEditionType.Book)
            || (getNewspaper && edition.Type == Enums.PrintingEditionType.Newspaper)
            || (getJournal && edition.Type == Enums.PrintingEditionType.Journal);
            var dbPrintingEditions = _printingEditionRepository.GetAll(filter).ToList();
            var lastPage = (int)Math.Ceiling(dbPrintingEditions.Count / (double)Constants.ADMINPRINTINGEDITIONPAGESIZE);

            return lastPage;
        }
        public PrintingEditionResponseModel GetPrintingEditions(int page = Constants.DEFAULTPAGE)
        {
            var dbPrintingEditions = _printingEditionRepository.Get(page: page);
            var printingEditions = _mapper.Map<List<PrintingEditionModel>>(dbPrintingEditions);
            var info = GetInfo();
            return new PrintingEditionResponseModel
            {
                Books = printingEditions,
                Info = info
            };
        }
        public List<PrintingEditionEntity> GetPrintingEditionsRange(Expression<Func<PrintingEditionEntity, bool>> filter = null, bool getRemoved = false)
        {
            var dbPrintingEditions = _printingEditionRepository.GetAll(filter, getRemoved);
            return dbPrintingEditions.ToList();
        }
        public PrintingEditionModel GetPrintingEdition(int id)
        {
            var result = _mapper.Map<PrintingEditionModel>(_printingEditionRepository.GetById(id));
            return result;
        }
        public PrintingEditionModel GetPrintingEditionByTitle(string title)
        {
            var result = _mapper.Map<PrintingEditionModel>(_printingEditionRepository.Get(edition => edition.Title == title).First());
            return result;
        }
    }
}
