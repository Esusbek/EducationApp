using EducationApp.BusinessLogicLayer.Models.Orders;
using EducationApp.Shared.Constants;
using System.Collections.Generic;

namespace EducationApp.BusinessLogicLayer.Models.ViewModels
{
    public class OrdersViewModel
    {
        public List<OrderModel> Orders { get; set; }
        public int Page { get; set; }
        public int PageCount { get; set; }
        public bool IsPaid { get; set; }
        public bool IsUnpaid { get; set; }
        public string SortBy { get; set; }
        public string Ascending { get; set; }

        public OrdersViewModel()
        {
            Page = Constants.DEFAULTPAGE;
            IsPaid = true;
            IsUnpaid = true;
            SortBy = Constants.DEFAULTORDERSORT;
            Ascending = Constants.DEFAULTSORTORDER;
        }
    }
}
