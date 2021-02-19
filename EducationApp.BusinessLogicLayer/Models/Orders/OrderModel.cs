using EducationApp.Shared.Enums;
using System;
using System.Collections.Generic;

namespace EducationApp.BusinessLogicLayer.Models.Orders
{
    public class OrderModel : Base.BaseModel
    {
        public string Description { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public int PaymentId { get; set; }
        public Enums.OrderStatusType Status { get; set; }
        public List<OrderItemModel> CurrentItems { get; set; }
        public OrderModel()
        {
            CurrentItems = new List<OrderItemModel>();
        }
    }
}
