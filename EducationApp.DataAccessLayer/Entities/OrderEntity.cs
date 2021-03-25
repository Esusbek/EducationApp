using EducationApp.DataAccessLayer.Entities.Base;
using EducationApp.Shared.Enums;
using System;

namespace EducationApp.DataAccessLayer.Entities
{
    public class OrderEntity : BaseEntity
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public Enums.OrderStatusType Status { get; set; }
        public string UserId { get; set; }
        public virtual UserEntity User { get; set; }
        public int PaymentId { get; set; }
        public virtual PaymentEntity Payment { get; set; }
    }
}
