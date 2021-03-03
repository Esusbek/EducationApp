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
        [HttpGet("Order/GetAll")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public IActionResult GetAll([FromQuery] int page = Constants.DEFAULTPAGE)
        {
            var orders = _orderService.GetAllOrders(page);
            return Ok(orders);
        }
        [HttpGet("Order/Get")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,client")]
        public IActionResult Get([FromBody] UserModel user, [FromQuery] int page = Constants.DEFAULTPAGE)
        {
            var orders = _orderService.GetUserOrders(user, page);
            return Ok(orders);
        }
        [HttpGet("Order/GetFiltered")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public IActionResult GetFiltered([FromBody] OrderFilterModel filter, [FromQuery] int page = Constants.DEFAULTPAGE)
        {
            var orders = _orderService.GetOrdersFiltered(filter, page: page);
            return Ok(orders);
        }

        [HttpPost("Order/Checkout")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,client")]
        public IActionResult Checkout([FromBody] CheckoutRequestModel model)
        {
            var id = _orderService.CreateCheckoutSession(model.Order, model.User);
            return Json(new { id });
        }
        [HttpPost("Order/Success")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,client")]
        public IActionResult Success([FromBody] SuccessRequestModel model)
        {
            _orderService.PayOrder(model.Order, model.TransactionId);
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
