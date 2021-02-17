using EducationApp.Shared.Enums;
using System;

namespace EducationApp.DataAccessLayer.Entities
{
    public class OrderEntity : Base.BaseEntity
    {
        public string Description { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public int PaymentId { get; set; }
        public Enums.Order.Status Status { get; set; }

        public UserEntity User { get; set; }
        public PaymentEntity Payment { get; set; }
    }
}
