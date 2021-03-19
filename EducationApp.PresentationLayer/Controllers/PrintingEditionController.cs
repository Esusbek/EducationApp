using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using EducationApp.BusinessLogicLayer.Models.Requests;
using EducationApp.BusinessLogicLayer.Services.Interfaces;
using EducationApp.Shared.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
        [HttpGet("PrintingEdition/GetAll")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public IActionResult GetAll([FromQuery] int page = Constants.DEFAULTPAGE)
        {
            return Ok(_printingEditionService.GetPrintingEditionsFiltered(page: page, getRemoved: true));
        }
        [HttpGet("PrintingEdition/GetFiltered")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,client")]
        public IActionResult GetFiltered([FromQuery] PrintingEditionFilterModel filter, [FromQuery] bool orderAsc, [FromQuery] int page = Constants.DEFAULTPAGE)
        {
            return Ok(_printingEditionService.GetPrintingEditionsFiltered(filter, orderAsc, page));
        }
        [HttpPost("PrintingEdition/Edit")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public IActionResult Edit([FromBody] PrintingEditionModel printingEdition)
        {
            _printingEditionService.UpdatePrintingEdition(printingEdition);
            return Ok();
        }
        [HttpPost("PrintingEdition/Add")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public IActionResult Add([FromBody] PrintingEditionModel printingEdition)
        {
            _printingEditionService.AddPrintingEdition(printingEdition);
            return Ok();
        }
        [HttpPost("PrintingEdition/Delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public IActionResult Delete([FromBody] PrintingEditionModel printingEdition)
        {
            _printingEditionService.DeletePrintingEdition(printingEdition);
            return Ok();
        }
        [HttpPost("PrintingEdition/AddAuthor")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public IActionResult AddAuthor([FromBody] AuthorAndEditionRequestModel model)
        {
            _printingEditionService.AddAuthorToPrintingEdition(model.PrintingEdition, model.Author);
            return Ok();
        }
    }
}
