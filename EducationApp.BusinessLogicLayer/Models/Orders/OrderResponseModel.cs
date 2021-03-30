using System.Collections.Generic;

namespace EducationApp.BusinessLogicLayer.Models.Orders
{
    public class OrderResponseModel
    {
        public List<OrderModel> Orders { get; set; }
        public int LastPage { get; set; }
    }
}
