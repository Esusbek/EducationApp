using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EducationApp.DataAccessLayer.Repositories.Base
{
    public interface IBaseRepository<T> where T : Entities.Base.BaseEntity
    {
        T GetById(int id);
        IEnumerable<T> ListAll();
        IEnumerable<T> List();
        IEnumerable<T> List(Expression<Func<T, bool>> predicate);
        void Insert(T entity);
        void Delete(int id);
        void Update(T entity);
    }
}
