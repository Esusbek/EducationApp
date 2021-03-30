using EducationApp.BusinessLogicLayer.Models.Orders;
using EducationApp.BusinessLogicLayer.Models.Requests;
using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.BusinessLogicLayer.Services.Interfaces;
using EducationApp.Shared.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EducationApp.PresentationLayer.Controllers
{
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
       
        [HttpGet("Order/Get")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,client")]
        public IActionResult Get([FromQuery] UserModel user, [FromQuery] int page = Constants.DEFAULTPAGE)
        {
            var orders = _orderService.GetUserOrders(user, page);
            return Ok(orders);
        }
        [HttpGet("Order/GetFiltered")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public IActionResult GetFiltered([FromQuery] OrderFilterModel filter, [FromQuery] int page = Constants.DEFAULTPAGE)
        {
            var orders = _orderService.GetOrdersFiltered(filter, page: page);
            return Ok(orders);
        }

        [HttpPost("Order/Checkout")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,client")]
        public IActionResult Checkout([FromBody] OrderModel model)
        {
            var session = _orderService.CreateCheckoutSession(model);
            return Ok(session);
        }
        [HttpPost("Order/Success")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,client")]
        public IActionResult Success([FromBody] SuccessRequestModel model)
        {
            _orderService.PayOrder(model.PaymentIntentId);
            return Ok();
        }
        [HttpPost("Order/Cancel")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,client")]
        public IActionResult Cancel()
        {
            return Ok();
        }
        [HttpPost("Order/ConvertCurrency")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,client")]
        public async Task<IActionResult> ConvertCurrency([FromBody] CurrencyConvertRequestModel model)
        {
            var result = await _orderService.ConvertCurrencyAsync(model.FromCurrency, model.ToCurrency, model.Amount);
            return Ok(result);
        }
    }
}
