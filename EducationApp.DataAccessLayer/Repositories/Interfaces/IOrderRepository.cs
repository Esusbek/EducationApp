using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.FilterModels;
using EducationApp.DataAccessLayer.Repositories.Base.BaseInterface;
using EducationApp.Shared.Constants;
using System.Collections.Generic;

namespace EducationApp.DataAccessLayer.Repositories.Interfaces
{
    public interface IOrderRepository : IBaseRepository<OrderEntity>
    {
        public List<OrderEntity> Get(OrderFilterModel orderFilter = null, string field = Constants.DEFAULTORDERSORT, bool ascending = true, bool getRemoved = false, int page = Constants.DEFAULTPAGE);
        public List<OrderEntity> GetAll(OrderFilterModel orderFilter = null, bool getRemoved = false);
        public int GetCount(OrderFilterModel orderFilter, bool getRemoved = false);
    }
}
