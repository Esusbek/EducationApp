using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class OrderEntity: Base.BaseEntity
    {
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public int PaymentId { get; set; }

        public UserEntity User { get; set; }
        public PaymentEntity Payment { get; set; }
    }
}
