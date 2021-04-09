using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repositories.Base.BaseInterface;
using EducationApp.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EducationApp.DataAccessLayer.Repositories.Interfaces
{
    public interface IOrderRepository : IBaseRepository<OrderEntity>
    {
        public List<OrderEntity> Get(Expression<Func<OrderEntity, bool>> filter = null,
            string field = null, bool ascending = true,
            bool getRemoved = false,
            int page = Constants.DEFAULTPAGE);
        public List<OrderEntity> GetAll(Expression<Func<OrderEntity, bool>> filter = null,
            bool getRemoved = false);
    }
}
