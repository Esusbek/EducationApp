﻿using EducationApp.BusinessLogicLayer.Models.Orders;
using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Services.Interfaces
{
    public interface IOrderService
    {
        public void PayOrder(string paymentIntentId);
        public SessionModel CreateCheckoutSession(OrderModel order);
        public List<OrderModel> GetAllOrders(bool getPaid = true, bool getUnpaid = true,string field = null, bool ascending = true,
            int page = Constants.DEFAULTPAGE, bool getRemoved = true);
        public OrderResponseModel GetUserOrders(UserModel user, int page = Constants.DEFAULTPAGE);
        public int GetLastPage(bool getPaid = true, bool getUnpaid = true);
        public Task<decimal> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount);
    }
}
