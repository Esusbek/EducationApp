using Dapper.Contrib.Extensions;
using EducationApp.DataAccessLayer.Entities.Base;
using EducationApp.DataAccessLayer.Extensions;
using EducationApp.DataAccessLayer.Repositories.Base.BaseInterface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace EducationApp.DataAccessLayer.Repositories.Base
{
    public class BaseDapperRepository<T> : IBaseDapperRepository<T> where T : BaseEntity
    {
        private readonly string _connectionString;
        public BaseDapperRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }
        public virtual T GetById(int id)
        {
            using SqlConnection connection = new(_connectionString);
            var result = connection.Get<T>(id);
            return result;
        }
        public virtual List<T> GetAll()
        {
            using SqlConnection connection = new(_connectionString);
            var result = connection.GetAll<T>();
            return result.ToList();
        }
        public virtual List<T> Get(Expression<Func<T, bool>> filter = null, string field = null, bool ascending = true, bool getRemoved = false)
        {
            using SqlConnection connection = new(_connectionString);
            var query = connection.GetAll<T>().AsQueryable();
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
            return query.ToList();
        }
        public virtual T GetOne(Expression<Func<T, bool>> filter = null, string field = null, bool ascending = true, bool getRemoved = false)
        {
            using SqlConnection connection = new(_connectionString);
            var query = connection.GetAll<T>().AsQueryable();
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
            using SqlConnection connection = new(_connectionString);
            connection.Insert(entity);
        }

        public virtual void InsertRange(IEnumerable<T> entity)
        {
            using SqlConnection connection = new(_connectionString);
            connection.Insert(entity);
        }
        public virtual void Update(T entity)
        {
            using SqlConnection connection = new(_connectionString);
            connection.Update(entity);
        }
        public virtual void Delete(T entityToDelete)
        {
            entityToDelete.IsRemoved = true;
            Update(entityToDelete);
        }
    }
}
