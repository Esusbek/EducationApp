using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.FilterModels;
using EducationApp.DataAccessLayer.Repositories.Base;
using EducationApp.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EducationApp.DataAccessLayer.Repositories.EFRepositories
{
    public class OrderItemRepository : BaseRepository<OrderItemEntity>, IOrderItemRepository
    {
        public OrderItemRepository(ApplicationContext dbContext)
            : base(dbContext)
        {
        }
        public List<OrderItemEntity> Get(OrderItemFilterModel orderItemFilterModel = null)
        {
            Expression<Func<OrderItemEntity, bool>> filter = null;
            if (orderItemFilterModel is not null)
            {
                filter = item => !orderItemFilterModel.OrderIds.Any() || orderItemFilterModel.OrderIds.Contains(item.OrderId);
            }
            return base.Get(filter).ToList();
        }
    }
}
