using EducationApp.BusinessLogicLayer.Models.Authors;
using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using EducationApp.BusinessLogicLayer.Services.Interfaces;
using EducationApp.Shared.Constants;
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
        public IActionResult Get(int page = Constants.Pages.InitialPage)
        {
            return Ok(_printingEditionService.GetPrintingEditions(page));
        }
        [HttpGet]
        public IActionResult GetAll(int page = Constants.Pages.InitialPage)
        {
            return Ok(_printingEditionService.GetPrintingEditionsFiltered(page: page, getRemoved: true));
        }
        [HttpGet]
        public IActionResult GetFiltered(PrintingEditionFilterModel filter, int page = Constants.Pages.InitialPage)
        {
            return Ok(_printingEditionService.GetPrintingEditionsFiltered(filter, page: page));
        }
        [HttpPost]
        public IActionResult Edit(PrintingEditionModel printingEdition)
        {
            _printingEditionService.UpdatePrintingEdition(printingEdition);
            return Ok();
        }
        [HttpPost]
        public IActionResult Add(PrintingEditionModel printingEdition)
        {
            _printingEditionService.AddPrintingEdition(printingEdition);
            return Ok();
        }
        [HttpPost]
        public IActionResult Delete(PrintingEditionModel printingEdition)
        {
            _printingEditionService.DeletePrintingEdition(printingEdition);
            return Ok();
        }
        [HttpPost]
        public IActionResult AddAuthor(PrintingEditionModel printingEdition, AuthorModel author)
        {
            _printingEditionService.AddAuthorToPrintingEdition(printingEdition, author);
            return Ok();
        }
    }
}
