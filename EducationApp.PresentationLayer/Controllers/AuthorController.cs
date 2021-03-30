using EducationApp.BusinessLogicLayer.Models.Authors;
using EducationApp.BusinessLogicLayer.Models.Requests;
using EducationApp.BusinessLogicLayer.Services.Interfaces;
using EducationApp.Shared.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,client")]
        public IActionResult Get([FromQuery] int page = Constants.DEFAULTPAGE)
        {
            return Ok(_authorService.GetAuthors(page));
        }
        [HttpGet("Author/GetAll")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,client")]
        public IActionResult GetAll([FromQuery] int page = Constants.DEFAULTPAGE)
        {
            return Ok(_authorService.GetAuthorsFiltered(page: page, getRemoved: true));
        }
        [HttpGet("Author/GetFiltered")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,client")]
        public IActionResult GetFiltered([FromQuery] AuthorFilterModel filter, [FromQuery] int page = Constants.DEFAULTPAGE)
        {
            return Ok(_authorService.GetAuthorsFiltered(filter, page: page));
        }
    }
}
