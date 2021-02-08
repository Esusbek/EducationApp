using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class OrderItemEntity: Base.BaseEntity
    {
        public int Amount { get; set; }
        public int OrderId { get; set; }
        public int PrintingEditionId { get; set; }
        public decimal SubTotal { get; set; }

        public OrderEntity Order { get; set; }
        public Enums.Enums.Currency Currency { get; set; }
        public PrintingEditionEntity PrintingEdition { get; set; }
    }
}
