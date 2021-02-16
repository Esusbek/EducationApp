using AutoMapper;
using EducationApp.BusinessLogicLayer.Models.Authors;
using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repositories;
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
        public PrintingEditionService(ApplicationContext context, IMapper mapper)
        {
            _printingEditionRepository = new PrintingEditionRepository(context);
            _authorRepository = new AuthorRepository(context);
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
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.Errors.PrintingEditionNotFound);
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
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.Errors.PrintingEditionNotFound);
            }
            var dbAuthor = _authorRepository.GetById(author.Id);
            if (dbAuthor is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.Errors.AuthorNotFound);
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
            int page = Constants.Pages.InitialPage, bool getRemoved = false)
        {
            Expression<Func<PrintingEditionEntity, bool>> filter = null;
            if (printingEditionFilter is not null)
            {
                filter = pe => (string.IsNullOrWhiteSpace(printingEditionFilter.Title) || pe.Title.Contains(printingEditionFilter.Title)) &&
                (printingEditionFilter.LowPrice == 0 || pe.Price > printingEditionFilter.LowPrice) &&
                (printingEditionFilter.HighPrice == 0 || pe.Price < printingEditionFilter.HighPrice);
            }
            var dbPrintingEditions = _printingEditionRepository.Get(filter, getRemoved: getRemoved, page: page);
            var printingEditions = new List<PrintingEditionModel>();
            foreach (var printingEdition in dbPrintingEditions)
            {
                printingEditions.Add(_mapper.Map<PrintingEditionModel>(printingEdition));
            }
            return printingEditions;
        }
        public List<PrintingEditionModel> GetPrintingEditions(int page = Constants.Pages.InitialPage)
        {
            var dbPrintingEditions = _printingEditionRepository.Get(page: page);
            var printingEditions = new List<PrintingEditionModel>();
            foreach (var printingEdition in dbPrintingEditions)
            {
                printingEditions.Add(_mapper.Map<PrintingEditionModel>(printingEdition));
            }
            return printingEditions;
        }
        public PrintingEditionModel GetPrintingEdition(int id)
        {
            return _mapper.Map<PrintingEditionModel>(_printingEditionRepository.GetById(id));
        }
        public PrintingEditionModel GetPrintingEdition(string title)
        {
            return _mapper.Map<PrintingEditionModel>(_printingEditionRepository.Get(pe => pe.Title == title).First());
        }
    }
}
