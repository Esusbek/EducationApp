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
        public IEnumerable<OrderEntity> GetAll(int page = Constants.Pages.InitialPage)
        {
            return base.GetAll()
                .Skip((page - 1) * Constants.Pages.OrderPageSize)
                .Take(Constants.Pages.OrderPageSize).ToList();
        }
        public IEnumerable<OrderEntity> Get(Expression<Func<OrderEntity, bool>> filter = null,
            Func<IQueryable<OrderEntity>, IOrderedQueryable<OrderEntity>> orderBy = null,
            bool getRemoved = false,
            int page = Constants.Pages.InitialPage)
        {
            return base.Get(filter, orderBy, getRemoved)
                .Skip((page - 1) * Constants.Pages.OrderPageSize)
                .Take(Constants.Pages.OrderPageSize).ToList();
        }
    }
}
