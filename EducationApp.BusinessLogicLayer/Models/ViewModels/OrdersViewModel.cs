using EducationApp.BusinessLogicLayer.Models.Orders;
using EducationApp.Shared.Constants;
using System.Collections.Generic;

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
            CurrentPage = Constants.DEFAULTPAGE;
            GetPaid = true;
            GetUnpaid = true;
            SortBy = Constants.DEFAULTORDERSORT;
            Ascending = true;
        }
    }
}
