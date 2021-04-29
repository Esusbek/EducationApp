using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.FilterModels;
using EducationApp.DataAccessLayer.Repositories.Base.BaseInterface;
using System.Collections.Generic;

namespace EducationApp.DataAccessLayer.Repositories.Interfaces
{
    public interface IPaymentRepository: IBaseRepository<PaymentEntity>
    {
        public List<PaymentEntity> Get(PaymentFilterModel paymentFilterModel = null);
    }
}
