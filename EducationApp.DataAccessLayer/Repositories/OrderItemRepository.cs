using EducationApp.DataAccessLayer.Entities;

namespace EducationApp.DataAccessLayer.Repositories
{
    public class OrderItemRepository : Base.BaseRepository<OrderItemEntity>, Interfaces.IOrderItemRepository
    {
        public OrderItemRepository(AppContext.ApplicationContext dbContext)
            : base(dbContext)
        {
        }
    }
}
