﻿using EducationApp.DataAccessLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EducationApp.DataAccessLayer.Repositories.Base.BaseInterface
{
    public interface IBaseDapperRepository<T> where T : BaseEntity
    {
        T GetById(int id);
        List<T> GetAll();
        List<T> Get(Expression<Func<T, bool>> filter = null,
            string field = null, bool ascending = true,
            bool getRemoved = false);
        void Insert(T entity);
        void InsertRange(IEnumerable<T> entity);
        void Update(T entity);
        void Delete(T entityToDelete);
    }
}
