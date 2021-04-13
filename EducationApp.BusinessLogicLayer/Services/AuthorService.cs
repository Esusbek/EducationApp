using AutoMapper;
using EducationApp.BusinessLogicLayer.Models.Authors;
using EducationApp.BusinessLogicLayer.Providers.Interfaces;
using EducationApp.BusinessLogicLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.FilterModels;
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
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly IValidationProvider _validator;
        public AuthorService(IMapper mapper,IAuthorRepository authorRepository, IValidationProvider validationProvider)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _validator = validationProvider;
        }
        public void AddAuthor(AuthorModel author)
        {
            _validator.ValidateAuthor(author);
            var dbAuthor = _authorRepository.Get(new AuthorFilterModel { Name = author.Name}).FirstOrDefault();
            if (dbAuthor is not null)
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.AUTHORALREADYEXISTSERROR);
            }
            dbAuthor = _mapper.Map<AuthorEntity>(author);

            _authorRepository.Insert(dbAuthor);
        }
        public void UpdateAuthor(AuthorModel author)
        {
            _validator.ValidateAuthor(author);
            var dbAuthor = _authorRepository.GetById(author.Id);
            if (dbAuthor is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.AUTHORNOTFOUNDERROR);
            }
            _mapper.Map(author, dbAuthor);
            _authorRepository.Update(dbAuthor);
        }
        
        public void DeleteAuthor(AuthorModel author)
        {
            var dbAuthor = _authorRepository.GetById(author.Id);
            if (dbAuthor is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.AUTHORNOTFOUNDERROR);
            }
            dbAuthor.PrintingEditions.Clear();
            _authorRepository.Delete(dbAuthor);
        }
        public List<AuthorModel> GetAuthorsFiltered(AuthorFilterModel authorFilter = null, string field = null, string ascending = Constants.DEFAULTSORTORDER, int page = Constants.DEFAULTPAGE, bool getRemoved = false)
        {
            
            var dbAuthors = _authorRepository.Get(authorFilter, field, ascending == Constants.DEFAULTSORTORDER, getRemoved, page);
            var authors = _mapper.Map<List<AuthorModel>>(dbAuthors);
            return authors;
        }
        public List<AuthorModel> GetAuthors(int page = Constants.DEFAULTPAGE)
        {
            var dbAuthors = _authorRepository.Get(page: page);
            var authors = _mapper.Map<List<AuthorModel>>(dbAuthors);
            return authors;
        }
        public List<AuthorModel> GetAllAuthors()
        {
            var dbAuthors = _authorRepository.GetAll();
            var authors = _mapper.Map<List<AuthorModel>>(dbAuthors);
            return authors;
        }
        public int GetLastPage(AuthorFilterModel authorFilter = null, bool getRemoved = false)
        {
            var authors = _authorRepository.GetAll(authorFilter, getRemoved);
            int lastPage = (int)Math.Ceiling(authors.Count / (double)Constants.AUTHORPAGESIZE);
            return lastPage;
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
            var dbAuthor = _authorRepository.Get(new AuthorFilterModel { Name = name }).FirstOrDefault();
            if (dbAuthor is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.AUTHORNOTFOUNDERROR);
            }
            var author = _mapper.Map<AuthorModel>(dbAuthor);
            return author;
        }
    }
}
