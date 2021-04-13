using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.FilterModels;
using EducationApp.DataAccessLayer.Repositories.Base;
using EducationApp.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EducationApp.DataAccessLayer.Repositories.EFRepositories
{
    public class PaymentRepository : BaseRepository<PaymentEntity>, IPaymentRepository
    {
        public PaymentRepository(ApplicationContext dbContext)
            : base(dbContext)
        {
        }
        public List<PaymentEntity> Get(PaymentFilterModel paymentFilterModel = null)
        {
            Expression<Func<PaymentEntity, bool>> filter = null;
            if (paymentFilterModel is not null)
            {
                filter = payment => string.IsNullOrWhiteSpace(paymentFilterModel.TransactionId) || payment.TransactionId == paymentFilterModel.TransactionId;
            }
            return base.Get(filter);
        }
    }
}
