using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EducationApp.DataAccessLayer.Repositories.Base.BaseInterface
{
    public interface IBaseRepository<T> where T : Entities.Base.BaseEntity
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Get(Expression<Func<T, bool>> filter=null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy=null,
            bool getRemoved = false);
        void Insert(T entity);
        void InsertRange(IEnumerable<T> entity);
        int InsertAndReturnId(T entity);
        void Delete(string id);
        void Update(T entity);
        void Delete(T entityToDelete);
        void SaveChanges();
    }
}
