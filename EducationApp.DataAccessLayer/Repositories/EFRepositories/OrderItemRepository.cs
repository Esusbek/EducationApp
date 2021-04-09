using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repositories.Base;
using EducationApp.DataAccessLayer.Repositories.Interfaces;

namespace EducationApp.DataAccessLayer.Repositories.EFRepositories
{
    public class OrderItemRepository : BaseRepository<OrderItemEntity>, IOrderItemRepository
    {
        public OrderItemRepository(ApplicationContext dbContext)
            : base(dbContext)
        {
        }
    }
}
