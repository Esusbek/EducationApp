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
    public class AuthorService : Interfaces.IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IPrintingEditionRepository _printingEditionRepository;
        private readonly IMapper _mapper;
        public AuthorService(IMapper mapper,
            IAuthorRepository authorRepository, IPrintingEditionRepository printingEditionRepository)
        {
            _authorRepository = authorRepository;
            _printingEditionRepository = printingEditionRepository;
            _mapper = mapper;
        }
        public void AddAuthor(AuthorModel author)
        {
            var dbAuthor = _authorRepository.Get(author => author.Name == author.Name).FirstOrDefault();
            if (dbAuthor is not null)
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.AUTHORALREADYEXISTSERROR);
            }
            dbAuthor = _mapper.Map<AuthorEntity>(author);

            _authorRepository.Insert(dbAuthor);
        }
        public void UpdateAuthor(AuthorModel author)
        {
            var dbAuthor = _authorRepository.GetById(author.Id);
            if (dbAuthor is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.AUTHORNOTFOUNDERROR);
            }
            var updatedAuthor = _mapper.Map<AuthorEntity>(author);
            updatedAuthor.CreatedAt = dbAuthor.CreatedAt;
            _authorRepository.Update(updatedAuthor);
        }
        public void AddPrintingEditionToAuthor(AuthorModel author, PrintingEditionModel printingEdition)
        {
            var dbAuthor = _authorRepository.GetById(author.Id);
            if (dbAuthor is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.AUTHORNOTFOUNDERROR);
            }
            var dbPrintingEdition = _printingEditionRepository.GetById(printingEdition.Id);
            if (dbPrintingEdition is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.PRINTINGEDITIONNOTFOUNDERROR);
            }
            dbAuthor.PrintingEditions = new List<PrintingEditionEntity>();
            _authorRepository.AddPrintingEditionToAuthor(dbAuthor, dbPrintingEdition);
        }
        public void DeleteAuthor(AuthorModel author)
        {
            var dbAuthor = _authorRepository.GetById(author.Id);
            if (dbAuthor is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.AUTHORNOTFOUNDERROR);
            }
            _authorRepository.Delete(dbAuthor);
        }
        public List<AuthorModel> GetAuthorsFiltered(AuthorFilterModel authorFilter,
            Func<IQueryable<AuthorEntity>, IOrderedQueryable<AuthorEntity>> orderBy = null,
            int page = Constants.DEFAULTPAGE, bool getRemoved = false)
        {
            Expression<Func<AuthorEntity, bool>> filter = null;
            if (authorFilter != null)
            {
                filter = author => string.IsNullOrWhiteSpace(authorFilter.Name) || author.Name.Contains(authorFilter.Name);
            }
            var dbAuthors = _authorRepository.Get(filter, orderBy, getRemoved, page);
            var authors = new List<AuthorModel>();
            foreach (var author in dbAuthors)
            {
                authors.Add(_mapper.Map<AuthorModel>(author));
            }
            return authors;
        }
        public List<AuthorModel> GetAuthors(int page = Constants.DEFAULTPAGE)
        {
            var dbAuthors = _authorRepository.Get(page: page);
            var authors = new List<AuthorModel>();
            foreach (var author in dbAuthors)
            {
                authors.Add(_mapper.Map<AuthorModel>(author));
            }
            return authors;
        }
        public AuthorModel GetAuthor(int id)
        {
            var dbAuthor = _authorRepository.GetById(id);
            if (dbAuthor is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.AUTHORNOTFOUNDERROR);
            }
            var author = _mapper.Map<AuthorModel>(dbAuthor);
            return author;
        }
        public AuthorModel GetAuthorByName(string name)
        {
            var dbAuthor = _authorRepository.Get(author => author.Name == name).FirstOrDefault();
            if (dbAuthor is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.AUTHORNOTFOUNDERROR);
            }
            var author = _mapper.Map<AuthorModel>(dbAuthor);
            return author;
        }
    }
}
