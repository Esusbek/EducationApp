using EducationApp.Shared.Enums;
using System.Collections.Generic;

namespace EducationApp.BusinessLogicLayer.Models.Orders
{
    public class OrderFilterModel
    {
        public List<Enums.OrderStatusType> Status { get; set; }
    }
}
