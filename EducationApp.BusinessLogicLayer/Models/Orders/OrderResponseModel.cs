using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Models.Orders
{
    public class OrderResponseModel
    {
        public List<OrderModel> Orders { get; set; }
        public int LastPage { get; set; }
    }
}
