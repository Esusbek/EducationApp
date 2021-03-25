using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EducationApp.DataAccessLayer.Repositories.Base
{
    public class BaseRepository<T> : BaseInterface.IBaseRepository<T> where T : Entities.Base.BaseEntity
    {
        private readonly AppContext.ApplicationContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        public BaseRepository(AppContext.ApplicationContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }
        public virtual T GetById(int id)
        {
            return _dbSet.Find(id);
        }
        public virtual List<T> GetAll()
        {
            return _dbSet.ToList();
        }
        public virtual List<T> Get(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            bool getRemoved = false)
        {
            IQueryable<T> query = _dbSet;
            if (!getRemoved)
            {
                query = query.Where(entity => entity.IsRemoved == false);
            }
            if (filter is not null)
            {
                query = query.Where(filter);
            }

            if (orderBy is not null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }
        public virtual void Insert(T entity)
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
        }
        
        public virtual void InsertRange(IEnumerable<T> entity)
        {
            _dbSet.AddRange(entity);
            _dbContext.SaveChanges();
        }
        public virtual void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
        public virtual void Delete(T entityToDelete)
        {
            entityToDelete.IsRemoved = true;
            Update(entityToDelete);
        }
        public virtual void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
