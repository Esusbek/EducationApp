﻿using AutoMapper;
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
        public PrintingEditionResponseModel GetPrintingEditionsFiltered(PrintingEditionFilterModel printingEditionFilter = null,
            bool orderAsc = false, int page = Constants.DEFAULTPAGE, bool getRemoved = false)
        {
            Expression<Func<PrintingEditionEntity, bool>> filter = null;
            Func<IQueryable<PrintingEditionEntity>, IOrderedQueryable<PrintingEditionEntity>> orderBy = editions => editions.OrderByDescending(edition => edition.Price);
            if (printingEditionFilter is not null)
            {
                filter = edition => (string.IsNullOrWhiteSpace(printingEditionFilter.Title) || edition.Title.Contains(printingEditionFilter.Title)) &&
                (printingEditionFilter.LowPrice == 0 || edition.Price >= printingEditionFilter.LowPrice) &&
                (printingEditionFilter.HighPrice == 0 || edition.Price <= printingEditionFilter.HighPrice) &&
                (printingEditionFilter.Type == default || printingEditionFilter.Type.Contains(edition.Type));
            }
            if (orderAsc)
            {
               orderBy = editions => editions.OrderBy(edition=>edition.Price);
            }
            var dbPrintingEditions = _printingEditionRepository.Get(filter, orderBy, getRemoved, page).ToList();
            var printingEditions = new List<PrintingEditionModel>();
            foreach (var printingEdition in dbPrintingEditions)
            {
                var mappedEdition = _mapper.Map<PrintingEditionModel>(printingEdition);
                mappedEdition.Authors = new List<string>();
                foreach (var author in printingEdition.Authors)
                {
                    mappedEdition.Authors.Add(author.Name);
                }
                printingEditions.Add(mappedEdition);
                
            }
            var info = GetInfo(printingEditionFilter);
            return new PrintingEditionResponseModel
            {
                Books = printingEditions,
                Info = info
            };
        }
        public PrintingEditionsInfoModel GetInfo(PrintingEditionFilterModel printingEditionFilter = null)
        {
            Expression<Func<PrintingEditionEntity, bool>> filter = null;
            if (printingEditionFilter is not null)
            {
                filter = edition => (string.IsNullOrWhiteSpace(printingEditionFilter.Title) || edition.Title.Contains(printingEditionFilter.Title)) &&
                (printingEditionFilter.LowPrice == 0 || edition.Price >= printingEditionFilter.LowPrice) &&
                (printingEditionFilter.HighPrice == 0 || edition.Price <= printingEditionFilter.HighPrice) &&
                (printingEditionFilter.Type == default || printingEditionFilter.Type.Contains(edition.Type));
            }
            var dbPrintingEditions = _printingEditionRepository.GetNoPagination(filter).ToList();
            var lastPage = (int)Math.Ceiling(dbPrintingEditions.Count / (double)Constants.PRINTINGEDITIONPAGESIZE);
            if (printingEditionFilter is not null)
            {
                filter = edition => (string.IsNullOrWhiteSpace(printingEditionFilter.Title) || edition.Title.Contains(printingEditionFilter.Title)) &&
                (printingEditionFilter.Type == default || printingEditionFilter.Type.Contains(edition.Type));
            }
            dbPrintingEditions = _printingEditionRepository.GetNoPagination(filter).ToList();
            var min = dbPrintingEditions.Aggregate((currentMin, x) => (currentMin == null || x.Price < currentMin.Price ? x : currentMin)).Price;
            var max = dbPrintingEditions.Aggregate((currentMax, x) => (currentMax == null || x.Price > currentMax.Price ? x : currentMax)).Price;
            
            return new PrintingEditionsInfoModel
            {
                MaxPrice = max,
                MinPrice = min,
                LastPage = lastPage
            };
        }
        public PrintingEditionResponseModel GetPrintingEditions(int page = Constants.DEFAULTPAGE)
        {
            var dbPrintingEditions = _printingEditionRepository.Get(page: page);
            var printingEditions = new List<PrintingEditionModel>();
            foreach (var printingEdition in dbPrintingEditions)
            {
                printingEditions.Add(_mapper.Map<PrintingEditionModel>(printingEdition));
            }
            var info = GetInfo();
            return new PrintingEditionResponseModel
            {
                Books = printingEditions,
                Info = info
            };
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
