using Dapper.Contrib.Extensions;
using EducationApp.DataAccessLayer.Entities.Base;
using EducationApp.DataAccessLayer.Extensions;
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
    public class BaseDapperRepository<T> where T : BaseEntity
    {
        private readonly string _connectionString;
        public BaseDapperRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }
        public virtual T GetById(int? id)
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
