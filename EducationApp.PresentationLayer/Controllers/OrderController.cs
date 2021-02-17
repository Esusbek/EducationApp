using EducationApp.BusinessLogicLayer.Models.Orders;
using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.BusinessLogicLayer.Services.Interfaces;
using EducationApp.Shared.Constants;
using EducationApp.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationApp.PresentationLayer.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll(int page = Constants.Defaults.DefaultPage)
        {
            var orders = _orderService.GetAllOrders(page);
            return Ok(orders);
        }
        [HttpGet]
        public IActionResult Get(UserModel user,int page = Constants.Defaults.DefaultPage)
        {
            var orders = _orderService.GetUserOrders(user,page);
            return Ok(orders);
        }
        [HttpGet]
        public IActionResult ConfirmOrder(OrderModel order, UserModel user)
        {

            _orderService.ConfirmOrder(order, user);
            return Ok();
        }
        [HttpPost]
        public IActionResult Checkout([FromBody]OrderModel order)
        {
            var id = _orderService.CreateCheckoutSession(order);
            return Json(new { id });
        }
        [HttpPost]
        public IActionResult Success()
        {
            return Ok();
        }
        [HttpPost]
        public IActionResult Cancel()
        {
            var user = HttpContext.User;
            return Ok();
        }
    }
}
