using EducationApp.DataAccessLayer.Entities;

namespace EducationApp.DataAccessLayer.Repositories
{
    public class PaymentRepository : Base.BaseRepository<PaymentEntity>, Interfaces.IPaymentRepository
    {
        public PaymentRepository(AppContext.ApplicationContext dbContext)
            : base(dbContext)
        {
        }
    }
}
