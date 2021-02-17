using EducationApp.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Models.Orders
{
    public class OrderModel: Base.BaseModel
    {
        public string Description { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public int PaymentId { get; set; }
        public Enums.Order.Status Status { get; set; }
        public List<OrderItemModel> CurrentItems;
        public OrderModel()
        {
            CurrentItems = new List<OrderItemModel>();
        }
    }
}
