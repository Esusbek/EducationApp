using EducationApp.BusinessLogicLayer.Models.Authors;
using EducationApp.BusinessLogicLayer.Models.Requests;
using EducationApp.BusinessLogicLayer.Services.Interfaces;
using EducationApp.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationApp.PresentationLayer.Controllers
{
    [ApiController]
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        [HttpGet("Author/Get")]
        [Authorize(Roles = "admin,client")]
        public IActionResult Get([FromQuery] int page = Constants.DEFAULTPAGE)
        {
            return Ok(_authorService.GetAuthors(page));
        }
        [HttpGet("Author/GetAll")]
        [Authorize(Roles = "admin,client")]
        public IActionResult GetAll([FromQuery] int page = Constants.DEFAULTPAGE)
        {
            return Ok(_authorService.GetAuthorsFiltered(page: page, getRemoved: true));
        }
        [HttpGet("Author/GetFiltered")]
        [Authorize(Roles = "admin,client")]
        public IActionResult GetFiltered([FromBody] AuthorFilterModel filter, [FromQuery] int page = Constants.DEFAULTPAGE)
        {
            return Ok(_authorService.GetAuthorsFiltered(filter, page: page));
        }
        [HttpPost("Author/Edit")]
        [Authorize(Roles = "admin")]
        public IActionResult Edit([FromBody] AuthorModel author)
        {
            _authorService.UpdateAuthor(author);
            return Ok();
        }
        [HttpPost("Author/Add")]
        [Authorize(Roles = "admin")]
        public IActionResult Add([FromBody] AuthorModel author)
        {
            _authorService.AddAuthor(author);
            return Ok();
        }
        [HttpPost("Author/Delete")]
        [Authorize(Roles = "admin")]
        public IActionResult Delete([FromBody] AuthorModel author)
        {
            _authorService.DeleteAuthor(author);
            return Ok();
        }
        [HttpPost("Author/AddPrintingEdition")]
        [Authorize(Roles = "admin")]
        public IActionResult AddPrintingEdition([FromBody] AuthorAndEditionRequestModel model)
        {
            _authorService.AddPrintingEditionToAuthor(model.Author, model.PrintingEdition);
            return Ok();
        }
    }
}
