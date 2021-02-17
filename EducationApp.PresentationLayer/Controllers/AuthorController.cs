using EducationApp.BusinessLogicLayer.Models.Authors;
using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using EducationApp.BusinessLogicLayer.Services.Interfaces;
using EducationApp.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationApp.PresentationLayer.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        [HttpGet]
        public IActionResult Get(int page = 1)
        {
            return Ok(_authorService.GetAuthors(page));
        }
        [HttpGet]
        public IActionResult GetAll(int page = Constants.Defaults.DefaultPage)
        {
            return Ok(_authorService.GetAuthorsFiltered(page: page, getRemoved: true));
        }
        [HttpGet]
        public IActionResult GetFiltered(AuthorFilterModel filter, int page = Constants.Defaults.DefaultPage)
        {
            return Ok(_authorService.GetAuthorsFiltered(filter, page));
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Edit(AuthorModel author)
        {
            _authorService.UpdateAuthor(author);
            return Ok();
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Add(AuthorModel author)
        {
            _authorService.AddAuthor(author);
            return Ok();
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(AuthorModel author)
        {
            _authorService.DeleteAuthor(author);
            return Ok();
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult AddPrintingEdition(AuthorModel author, PrintingEditionModel printingEdition)
        {
            _authorService.AddPrintingEditionToAuthor(author, printingEdition);
            return Ok();
        }
    }
}
