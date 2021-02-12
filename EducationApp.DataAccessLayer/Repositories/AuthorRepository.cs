using EducationApp.DataAccessLayer.Entities;
using EducationApp.Shared.Constants;
using EducationApp.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;

namespace EducationApp.DataAccessLayer.Repositories
{
    class AuthorRepository<T> : Interfaces.IAuthorRepository<T> where T: AuthorEntity
    {
        private readonly AppContext.ApplicationContext _dbContext;
        public AuthorRepository(AppContext.ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Delete(int id)
        {
            var author = _dbContext.Authors.Find(id);
            if (author == null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.Errors.AuthorNotFound);
            }
            _dbContext.Remove(author);
            _dbContext.SaveChanges();
        }

        public AuthorEntity GetById(int id)
        {
            var author = _dbContext.Authors.Find(id);
            if (author == null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.Errors.AuthorNotFound);
            }
            return author;
        }

        public void Insert(AuthorEntity entity)
        {
            var dbAuthor = _dbContext.Authors.Find(entity.Id);
            if(dbAuthor==null)
            {
                _dbContext.Authors.Add(entity);
                _dbContext.SaveChanges();
            }
        }
        public IEnumerable<AuthorEntity> ListAll()
        {
            return _dbContext.Authors.ToList();
        }
        public IEnumerable<AuthorEntity> List(int page)
        {
            return _dbContext.Authors.Where(a=>a.IsRemoved==false).ToList();
        }

        public IEnumerable<AuthorEntity> List(Expression<Func<AuthorEntity, bool>> predicate)
        {
            return _dbContext.Authors.Where(a => a.IsRemoved == false).Where(predicate).ToList();
        }

        public void Update(AuthorEntity entity)
        {
            var dbAuthor = _dbContext.Authors.Find(entity.Id);
            if (dbAuthor == null)
            {
                _dbContext.Authors.Update(entity);
                _dbContext.SaveChanges();
            }   
        }
    }
}
