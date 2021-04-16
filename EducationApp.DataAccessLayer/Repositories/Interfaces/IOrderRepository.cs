using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.FilterModels;
using EducationApp.Shared.Constants;
using System.Collections.Generic;

namespace EducationApp.DataAccessLayer.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public List<OrderEntity> Get(OrderFilterModel orderFilter = null, string field = null, bool ascending = true, bool getRemoved = false, int page = Constants.DEFAULTPAGE);
        public List<OrderEntity> GetAll(OrderFilterModel orderFilter = null, bool getRemoved = false);
        public OrderEntity GetById(int id);
        public void Insert(OrderEntity entity);
        public void InsertRange(IEnumerable<OrderEntity> entity);
        public void Update(OrderEntity entity);
        public void Delete(OrderEntity entityToDelete);
    }
}
