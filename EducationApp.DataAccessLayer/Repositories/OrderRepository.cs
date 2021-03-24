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
        public IQueryable<OrderEntity> GetAll(int page = Constants.DEFAULTPAGE)
        {
            return base.GetAll()
                .Skip((page - Constants.DEFAULTPREVIOUSPAGEOFFSET) * Constants.ORDERPAGESIZE)
                .Take(Constants.ORDERPAGESIZE);
        }
        public IQueryable<OrderEntity> Get(Expression<Func<OrderEntity, bool>> filter = null,
            Func<IQueryable<OrderEntity>, IOrderedQueryable<OrderEntity>> orderBy = null,
            bool getRemoved = false,
            int page = Constants.DEFAULTPAGE)
        {
            return base.Get(filter, orderBy, getRemoved)
                .Skip((page - Constants.DEFAULTPREVIOUSPAGEOFFSET) * Constants.ORDERPAGESIZE)
                .Take(Constants.ORDERPAGESIZE);
        }
        public IQueryable<OrderEntity> GetNoPagination()
        {
            return base.Get();
        }
    }
}
