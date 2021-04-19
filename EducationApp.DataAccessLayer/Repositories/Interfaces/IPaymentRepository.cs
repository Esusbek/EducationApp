using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.FilterModels;
using System.Collections.Generic;

namespace EducationApp.DataAccessLayer.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        public List<PaymentEntity> Get(PaymentFilterModel paymentFilterModel = null);
        public PaymentEntity GetById(int id);
        public void Insert(PaymentEntity entity);
        public void InsertRange(IEnumerable<PaymentEntity> entity);
        public void Update(PaymentEntity entity);
        public void Delete(PaymentEntity entityToDelete);
        public void SaveChanges();
    }
}
