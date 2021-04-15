using EducationApp.DataAccessLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EducationApp.DataAccessLayer.Repositories.Base.BaseInterface
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        public T GetById(int id);
        public List<T> GetAll();
        public List<T> Get(Expression<Func<T, bool>> filter = null, string field = null, bool ascending = true, bool getRemoved = false);
        public T GetOne(Expression<Func<T, bool>> filter = null, string field = null, bool ascending = true, bool getRemoved = false);
        public void Insert(T entity);
        public void InsertRange(IEnumerable<T> entity);
        public void Update(T entity);
        public void Delete(T entityToDelete);
        public void SaveChanges();
    }
}
