using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.FilterModels;
using EducationApp.DataAccessLayer.Repositories.Base.BaseInterface;
using EducationApp.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EducationApp.DataAccessLayer.Repositories.Interfaces
{
    public interface IOrderRepository : IBaseRepository<OrderEntity>
    {
        public List<OrderEntity> Get(OrderFilterModel orderFilter = null,
            string field = null, bool ascending = true,
            bool getRemoved = false,
            int page = Constants.DEFAULTPAGE);
        public List<OrderEntity> GetAll(OrderFilterModel orderFilter = null,
            bool getRemoved = false);
    }
}
