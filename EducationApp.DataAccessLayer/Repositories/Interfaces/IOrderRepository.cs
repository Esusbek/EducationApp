using EducationApp.DataAccessLayer.Entities;
using EducationApp.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EducationApp.DataAccessLayer.Repositories.Interfaces
{
    public interface IOrderRepository : Base.BaseInterface.IBaseRepository<OrderEntity>
    {
        public List<OrderEntity> Get(Expression<Func<OrderEntity, bool>> filter = null,
            Func<IQueryable<OrderEntity>, IOrderedQueryable<OrderEntity>> orderBy = null,
            bool getRemoved = false,
            int page = Constants.DEFAULTPAGE);
    }
}
