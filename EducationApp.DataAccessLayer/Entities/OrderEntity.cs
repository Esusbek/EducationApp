using EducationApp.DataAccessLayer.Entities.Base;
using EducationApp.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationApp.DataAccessLayer.Entities
{
    public class OrderEntity : BaseEntity
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Total { get; set; }
        public Enums.OrderStatusType Status { get; set; }
        public string UserId { get; set; }
        public virtual UserEntity User { get; set; }
        public int PaymentId { get; set; }
        public virtual PaymentEntity Payment { get; set; }
    }
}
