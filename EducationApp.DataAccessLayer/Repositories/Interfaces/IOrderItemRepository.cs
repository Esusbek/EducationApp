using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.FilterModels;
using EducationApp.DataAccessLayer.Repositories.Base.BaseInterface;
using System.Collections.Generic;

namespace EducationApp.DataAccessLayer.Repositories.Interfaces
{
    public interface IOrderItemRepository: IBaseRepository<OrderItemEntity>
    {
        public List<OrderItemEntity> Get(OrderItemFilterModel orderItemFilterModel = null);
        public List<OrderItemEntity> GetAll();
    }
}
