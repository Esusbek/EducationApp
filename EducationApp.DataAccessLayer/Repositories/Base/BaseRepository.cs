using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EducationApp.DataAccessLayer.Repositories.Base
{
    class BaseRepository<T> : IBaseRepository<T> where T : Entities.Base.BaseEntity
    {
        private readonly AppContext.ApplicationContext _dbContext;
        public BaseRepository(AppContext.ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }
        public virtual T GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }
        public virtual IEnumerable<T> ListAll()
        {
            return _dbContext.Set<T>()
                .AsEnumerable();
        }
        public virtual IEnumerable<T> List()
        {
            return _dbContext.Set<T>()
                .Where(e => e.IsRemoved == false)
                .AsEnumerable();
        }
        public virtual IEnumerable<T> List(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>()
                   .Where(e => e.IsRemoved == false)
                   .Where(predicate)
                   .AsEnumerable();
        }
        public void Insert(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
        }
        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
        public void Delete(int id)
        {
            var entity = _dbContext.Set<T>().Find(id);
            if (entity != null)
            {
                entity.IsRemoved = true;
                _dbContext.Update(entity);
            }
            _dbContext.SaveChanges();
        }
    }
}
