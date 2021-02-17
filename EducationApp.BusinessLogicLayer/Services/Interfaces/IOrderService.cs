using EducationApp.BusinessLogicLayer.Models.Orders;
using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Services.Interfaces
{
    public interface IOrderService
    {
        public void ConfirmOrder(OrderModel currentOrder, UserModel user);
        public void PayOrder(OrderModel order, string transactionId);
        public string CreateCheckoutSession(OrderModel order);
        public List<OrderModel> GetAllOrders(int page = Constants.Defaults.DefaultPage);
        public List<OrderModel> GetUserOrders(UserModel user, int page = Constants.Defaults.DefaultPage);
    }
}
