﻿using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using EducationApp.BusinessLogicLayer.Models.Requests;
using EducationApp.BusinessLogicLayer.Services.Interfaces;
using EducationApp.Shared.Constants;
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
        [Authorize(Roles = "admin,client")]
        public IActionResult Get([FromQuery] int page = Constants.DEFAULTPAGE)
        {
            return Ok(_printingEditionService.GetPrintingEditions(page));
        }
        [HttpGet("PrintingEdition/GetAll")]
        [Authorize(Roles = "admin,client")]
        public IActionResult GetAll([FromQuery] int page = Constants.DEFAULTPAGE)
        {
            return Ok(_printingEditionService.GetPrintingEditionsFiltered(page: page, getRemoved: true));
        }
        [HttpGet("PrintingEdition/GetFiltered")]
        [Authorize(Roles = "admin,client")]
        public IActionResult GetFiltered([FromBody] PrintingEditionFilterModel filter, [FromQuery] int page = Constants.DEFAULTPAGE)
        {
            return Ok(_printingEditionService.GetPrintingEditionsFiltered(filter, page: page));
        }
        [HttpPost("PrintingEdition/Edit")]
        [Authorize(Roles = "admin")]
        public IActionResult Edit([FromBody] PrintingEditionModel printingEdition)
        {
            _printingEditionService.UpdatePrintingEdition(printingEdition);
            return Ok();
        }
        [HttpPost("PrintingEdition/Add")]
        [Authorize(Roles = "admin")]
        public IActionResult Add([FromBody] PrintingEditionModel printingEdition)
        {
            _printingEditionService.AddPrintingEdition(printingEdition);
            return Ok();
        }
        [HttpPost("PrintingEdition/Delete")]
        [Authorize(Roles = "admin")]
        public IActionResult Delete([FromBody] PrintingEditionModel printingEdition)
        {
            _printingEditionService.DeletePrintingEdition(printingEdition);
            return Ok();
        }
        [HttpPost("PrintingEdition/AddAuthor")]
        [Authorize(Roles = "admin")]
        public IActionResult AddAuthor([FromBody] AuthorAndEditionRequestModel model)
        {
            _printingEditionService.AddAuthorToPrintingEdition(model.PrintingEdition, model.Author);
            return Ok();
        }
    }
}
