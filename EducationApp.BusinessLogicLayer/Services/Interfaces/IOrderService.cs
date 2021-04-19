using EducationApp.BusinessLogicLayer.Models.Orders;
using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.BusinessLogicLayer.Models.ViewModels;
using EducationApp.Shared.Constants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Services.Interfaces
{
    public interface IOrderService
    {
        public OrdersViewModel GetViewModel(OrdersViewModel model);
        public void PayOrder(string paymentIntentId);
        public SessionModel CreateCheckoutSession(OrderModel order);
        public List<OrderModel> GetAllOrders(bool getPaid = true, bool getUnpaid = true, string field = null, string ascending = Constants.DEFAULTSORTORDER, int page = Constants.DEFAULTPAGE, bool getRemoved = false);
        public OrderResponseModel GetUserOrders(UserModel user, int page = Constants.DEFAULTPAGE);
        public int GetLastPage(bool getPaid = true, bool getUnpaid = true);
        public Task<decimal> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount);
    }
}
