using EducationApp.BusinessLogicLayer.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Models.ViewModels
{
    public class OrdersViewModel
    {
        public List<OrderModel> Orders { get; set; }
        public int CurrentPage { get; set; }
        public int LastPage { get; set; }
        public bool GetPaid { get; set; }
        public bool GetUnpaid { get; set; }
        public string SortBy { get; set; }
        public bool Ascending { get; set; }

        public OrdersViewModel()
        {
            CurrentPage = 1;
            GetPaid = true;
            GetUnpaid = true;
            SortBy = "Date";
            Ascending = true;
        }
    }
}
