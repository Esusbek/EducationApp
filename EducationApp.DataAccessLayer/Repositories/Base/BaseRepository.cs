﻿using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities.Base;
using EducationApp.DataAccessLayer.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EducationApp.DataAccessLayer.Repositories.Base
{
    public class BaseRepository<T> where T : BaseEntity
    {
        private readonly ApplicationContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        public BaseRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }
        public virtual T GetById(int? id)
        {
            return _dbSet.Find(id);
        }
        public virtual List<T> GetAll()
        {
            return _dbSet.ToList();
        }
        protected virtual IQueryable<T> Get(Expression<Func<T, bool>> filter = null, string field = null, bool ascending = true, bool getRemoved = false)
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
            if (field is not null)
            {
                query = query.OrderBy(field, ascending);
            }
            return query;
        }
        protected virtual T GetOne(Expression<Func<T, bool>> filter = null, string field = null, bool ascending = true, bool getRemoved = false)
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
            if (field is not null)
            {
                query = query.OrderBy(field, ascending);
            }
            return query.FirstOrDefault();
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
