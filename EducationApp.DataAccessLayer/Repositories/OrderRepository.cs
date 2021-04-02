using EducationApp.DataAccessLayer.Entities;
using EducationApp.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EducationApp.DataAccessLayer.Repositories
{
    public class OrderRepository : Base.BaseRepository<OrderEntity>, Interfaces.IOrderRepository
    {
        public OrderRepository(AppContext.ApplicationContext dbContext)
            : base(dbContext)
        {
        }
        public List<OrderEntity> Get(Expression<Func<OrderEntity, bool>> filter = null,
            string field = null, bool ascending = true,
            bool getRemoved = false,
            int page = Constants.DEFAULTPAGE)
        {
            return base.Get(filter, field, ascending, getRemoved)
                .Skip((page - Constants.DEFAULTPREVIOUSPAGEOFFSET) * Constants.ORDERPAGESIZE)
                .Take(Constants.ORDERPAGESIZE).ToList();
        }
        public List<OrderEntity> GetAll(Expression<Func<OrderEntity, bool>> filter = null,
            bool getRemoved = false)
        {
            return base.Get(filter, getRemoved: getRemoved);
        }
    }
}
