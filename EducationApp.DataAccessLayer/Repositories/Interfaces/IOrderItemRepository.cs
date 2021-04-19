using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.FilterModels;
using System.Collections.Generic;

namespace EducationApp.DataAccessLayer.Repositories.Interfaces
{
    public interface IOrderItemRepository
    {
        public List<OrderItemEntity> Get(OrderItemFilterModel orderItemFilterModel = null);
        public OrderItemEntity GetById(int id);
        public List<OrderItemEntity> GetAll();
        public void Insert(OrderItemEntity entity);
        public void InsertRange(IEnumerable<OrderItemEntity> entity);
        public void Update(OrderItemEntity entity);
        public void Delete(OrderItemEntity entityToDelete);
        public void SaveChanges();
    }
}
