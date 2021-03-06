﻿using AutoMapper;
using EducationApp.BusinessLogicLayer.Models.Authors;
using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using EducationApp.BusinessLogicLayer.Models.ViewModels;
using EducationApp.BusinessLogicLayer.Providers.Interfaces;
using EducationApp.BusinessLogicLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.FilterModels;
using EducationApp.DataAccessLayer.Repositories.Interfaces;
using EducationApp.Shared.Constants;
using EducationApp.Shared.Enums;
using EducationApp.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public PrintingEditionsViewModel GetViewModel(PrintingEditionsViewModel model)
        {
            return new PrintingEditionsViewModel
            {
                PrintingEditions = GetPrintingEditionsAdmin(model.IsBook, model.IsNewspaper, model.IsJournal, model.SortBy, model.Ascending),
                Page = model.Page,
                PageCount = GetPageCount(model.IsBook, model.IsNewspaper, model.IsJournal),
                Ascending = model.Ascending,
                Authors = GetAllAuthors(),
                IsBook = model.IsBook,
                IsJournal = model.IsJournal,
                IsNewspaper = model.IsNewspaper,
                SortBy = model.SortBy
            };
        }
        public void AddPrintingEdition(PrintingEditionModel printingEdition)
        {
            _validator.ValidatePrintingEdition(printingEdition);
            var dbPrintingEdition = _mapper.Map<PrintingEditionEntity>(printingEdition);
            dbPrintingEdition.Status = Enums.PrintingEditionStatusType.InStock;
            var authorFilter = new AuthorFilterModel { EditionAuthors = printingEdition.Authors };
            var authors = _authorRepository.GetAll(authorFilter);
            dbPrintingEdition.Authors = new List<AuthorEntity>(authors);
            _printingEditionRepository.Insert(dbPrintingEdition);
        }
        public void UpdatePrintingEdition(PrintingEditionModel printingEdition)
        {
            _validator.ValidatePrintingEdition(printingEdition);
            var dbPrintingEdition = _printingEditionRepository.GetById(printingEdition.Id);
            if (dbPrintingEdition is null)
            {
                throw new CustomApiException(HttpStatusCode.BadRequest, Constants.PRINTINGEDITIONNOTFOUNDERROR);
            }
            _mapper.Map(printingEdition, dbPrintingEdition);
            var authorFilter = new AuthorFilterModel { EditionAuthors = printingEdition.Authors };
            var authors = _authorRepository.GetAll(authorFilter);
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
                throw new CustomApiException(HttpStatusCode.BadRequest, Constants.PRINTINGEDITIONNOTFOUNDERROR);
            }
            _printingEditionRepository.Delete(dbPrintingEdition);

        }
        public List<PrintingEditionModel> GetPrintingEditionsAdmin(bool isBook = true, bool isNewspaper = true, bool isJournal = true, string field = Constants.DEFAULTEDITIONSORT, string orderAsc = Constants.DEFAULTSORTORDER, int page = Constants.DEFAULTPAGE, bool getRemoved = false)
        {
            var filter = new PrintingEditionFilterModel
            {
                IsBook = isBook,
                IsJournal = isJournal,
                IsNewspaper = isNewspaper
            };
            var dbPrintingEditions = _printingEditionRepository.Get(filter, field, orderAsc == Constants.DEFAULTSORTORDER, getRemoved, page, Constants.ADMINPRINTINGEDITIONPAGESIZE).ToList();
            var printingEditions = new List<PrintingEditionModel>();
            foreach (var printingEdition in dbPrintingEditions)
            {
                var mappedEdition = _mapper.Map<PrintingEditionModel>(printingEdition);
                mappedEdition.Authors = printingEdition.Authors.Select(author => author.Name).ToList();
                printingEditions.Add(mappedEdition);
            }
            return printingEditions;
        }
        public PrintingEditionResponseModel GetPrintingEditionsFiltered(PrintingEditionFilterModel printingEditionFilter = null, string field = Constants.DEFAULTEDITIONSORT, bool orderAsc = false, int page = Constants.DEFAULTPAGE, bool getRemoved = false, int pageSize = Constants.PRINTINGEDITIONPAGESIZE)
        {
            var dbPrintingEditions = _printingEditionRepository.Get(printingEditionFilter, field, orderAsc, getRemoved, page, pageSize).ToList();
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

            int dbPrintingEditionsCount = _printingEditionRepository.GetCount(printingEditionFilter);
            int pageCount = (int)Math.Ceiling(dbPrintingEditionsCount / (double)pageSize);
            var filter = new PrintingEditionFilterModel
            {
                Title = printingEditionFilter.Title,
                Type = printingEditionFilter.Type
            };
            var dbPrintingEditions = _printingEditionRepository.GetAll(filter).ToList();

            decimal min = default;
            decimal max = default;
            if (dbPrintingEditions.Any())
            {
                min = dbPrintingEditions.Aggregate((currentMin, x) => (currentMin == null || x.Price < currentMin.Price ? x : currentMin)).Price;
                max = dbPrintingEditions.Aggregate((currentMax, x) => (currentMax == null || x.Price > currentMax.Price ? x : currentMax)).Price;
            }
            return new PrintingEditionsInfoModel
            {
                MaxPrice = max,
                MinPrice = min,
                PageCount = pageCount
            };
        }
        public int GetPageCount(bool isBook = true, bool isNewspaper = true, bool isJournal = true)
        {
            var filter = new PrintingEditionFilterModel
            {
                IsBook = isBook,
                IsJournal = isJournal,
                IsNewspaper = isNewspaper
            };
            int dbPrintingEditionsCount = _printingEditionRepository.GetCount(filter);
            int pageCount = (int)Math.Ceiling(dbPrintingEditionsCount / (double)Constants.ADMINPRINTINGEDITIONPAGESIZE);

            return pageCount;
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
        public List<PrintingEditionEntity> GetPrintingEditionsRange(PrintingEditionFilterModel printingEditionFilter = null, bool getRemoved = false)
        {
            var dbPrintingEditions = _printingEditionRepository.GetAll(printingEditionFilter, getRemoved);
            return dbPrintingEditions.ToList();
        }
        public PrintingEditionModel GetPrintingEdition(int id)
        {
            var edition = _printingEditionRepository.GetById(id);
            var result = _mapper.Map<PrintingEditionModel>(edition);
            return result;
        }
        public PrintingEditionModel GetPrintingEditionByTitle(string title)
        {
            var editionFilter = new PrintingEditionFilterModel { Title = title };
            var edition = _printingEditionRepository.GetOne(editionFilter);
            var result = _mapper.Map<PrintingEditionModel>(edition);
            return result;
        }
        private List<AuthorModel> GetAllAuthors()
        {
            var dbAuthors = _authorRepository.GetAll();
            var authors = _mapper.Map<List<AuthorModel>>(dbAuthors);
            return authors;
        }
    }
}
