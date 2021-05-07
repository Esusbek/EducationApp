using EducationApp.DataAccessLayer.Entities.Base;
using System.Collections.Generic;

namespace EducationApp.DataAccessLayer.Repositories.Base.BaseInterface
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        public T GetById(int? id);
        public void Insert(T entity);
        public void InsertRange(IEnumerable<T> entity);
        public void Update(T entity);
        public void Delete(T entityToDelete);
        public void SaveChanges();
    }
}
