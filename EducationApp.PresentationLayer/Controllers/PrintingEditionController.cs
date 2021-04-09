using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using EducationApp.BusinessLogicLayer.Services.Interfaces;
using EducationApp.Shared.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationApp.PresentationLayer.Controllers
{
    [ApiController]
    public class PrintingEditionController : Controller
    {
        private readonly IPrintingEditionService _printingEditionService;
        public PrintingEditionController(IPrintingEditionService printingEditionService)
        {
            _printingEditionService = printingEditionService;
        }
        [HttpGet("PrintingEdition/Get")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,client")]
        public IActionResult Get([FromQuery] int page = Constants.DEFAULTPAGE)
        {
            return Ok(_printingEditionService.GetPrintingEditions(page));
        }
        [HttpGet("PrintingEdition/GetFiltered")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,client")]
        public IActionResult GetFiltered([FromQuery] PrintingEditionFilterModel filter, [FromQuery] bool orderAsc, [FromQuery] int page = Constants.DEFAULTPAGE)
        {
            return Ok(_printingEditionService.GetPrintingEditionsFiltered(filter, orderAsc: orderAsc, page: page));
        }
    }
}
