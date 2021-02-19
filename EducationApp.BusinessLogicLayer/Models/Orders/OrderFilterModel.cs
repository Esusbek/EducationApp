using EducationApp.Shared.Enums;
using System;

namespace EducationApp.BusinessLogicLayer.Models.Orders
{
    public class OrderFilterModel
    {
        public string Description { get; set; }
        public string UserId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public Enums.OrderStatusType Status { get; set; }
    }
}
