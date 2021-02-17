using EducationApp.BusinessLogicLayer.Models.Authors;
using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using EducationApp.BusinessLogicLayer.Services.Interfaces;
using EducationApp.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationApp.PresentationLayer.Controllers
{
    public class PrintingEditionController : Controller
    {
        private readonly IPrintingEditionService _printingEditionService;
        public PrintingEditionController(IPrintingEditionService printingEditionService)
        {
            _printingEditionService = printingEditionService;
        }
        public IActionResult Get(int page = Constants.Defaults.DefaultPage)
        {
            return Ok(_printingEditionService.GetPrintingEditions(page));
        }
        [HttpGet]
        public IActionResult GetAll(int page = Constants.Defaults.DefaultPage)
        {
            return Ok(_printingEditionService.GetPrintingEditionsFiltered(page: page, getRemoved: true));
        }
        [HttpGet]
        public IActionResult GetFiltered(PrintingEditionFilterModel filter, int page = Constants.Defaults.DefaultPage)
        {
            return Ok(_printingEditionService.GetPrintingEditionsFiltered(filter, page: page));
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Edit(PrintingEditionModel printingEdition)
        {
            _printingEditionService.UpdatePrintingEdition(printingEdition);
            return Ok();
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Add(PrintingEditionModel printingEdition)
        {
            _printingEditionService.AddPrintingEdition(printingEdition);
            return Ok();
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(PrintingEditionModel printingEdition)
        {
            _printingEditionService.DeletePrintingEdition(printingEdition);
            return Ok();
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult AddAuthor(PrintingEditionModel printingEdition, AuthorModel author)
        {
            _printingEditionService.AddAuthorToPrintingEdition(printingEdition, author);
            return Ok();
        }
    }
}
