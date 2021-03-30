using EducationApp.BusinessLogicLayer.Models.Authors;
using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using EducationApp.BusinessLogicLayer.Models.Requests;
using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.BusinessLogicLayer.Models.ViewModels;
using EducationApp.BusinessLogicLayer.Services.Interfaces;
using EducationApp.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EducationApp.PresentationLayer.Controllers
{
    [Authorize(Roles="admin")]
    public class AdminController : Controller
    {
        private readonly IAuthorService _authorService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IPrintingEditionService _printingEditionService;
        public AdminController(IAuthorService authorService, IOrderService orderService, IPrintingEditionService printingEditionService, IUserService userService)
        {
            _authorService = authorService;
            _userService = userService;
            _orderService = orderService;
            _printingEditionService = printingEditionService;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginViewModel model)
        {
            var user = new UserModel
            {
                Password = model.Password,
                UserName = model.UserName
            };
            await _userService.LoginAsync(user, true);
            return RedirectToAction("Profile");
        }
        public async Task<IActionResult> Profile()
        {
            return View(new ProfileViewModel { User = await _userService.GetUserByUsernameAsync("Admin")});
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromForm] UserModel user)
        {
            await _userService.UpdateAsync(user);
            return RedirectToAction("Profile");
        }
        [HttpGet]
        public IActionResult Users([FromQuery] int page = Constants.DEFAULTPAGE)
        {
            return View(_userService.GetUsers(page));
        }
        [HttpGet]
        public IActionResult PrintingEditions([FromQuery] int page = Constants.DEFAULTPAGE)
        {
            return View(_printingEditionService.GetPrintingEditionsFiltered(page: page, getRemoved: true, pageSize: Constants.ADMINPRINTINGEDITIONPAGESIZE));
        }
        [HttpGet]
        public IActionResult Orders([FromQuery] int page = Constants.DEFAULTPAGE)
        {
            return View(_orderService.GetAllOrders(page: page));
        }
        [HttpGet]
        public IActionResult Authors([FromQuery] int page = Constants.DEFAULTPAGE)
        {
            return View(_authorService.GetAuthorsFiltered(page: page, getRemoved: true));
        }
        public async Task<IActionResult> BanUser([FromBody] BanRequestModel model)
        {
            await _userService.BanAsync(model.User, model.Duration);
            return Ok();
        }
        [HttpPost]
        public IActionResult EditAuthor(AuthorModel author)
        {
            _authorService.UpdateAuthor(author);
            return Ok();
        }
        public IActionResult AddAuthor(AuthorModel author)
        {
            _authorService.AddAuthor(author);
            return Ok();
        }
        public IActionResult DeleteAuthor(AuthorModel author)
        {
            _authorService.DeleteAuthor(author);
            return Ok();
        }
        public IActionResult AddPrintingEdition(AuthorAndEditionRequestModel model)
        {
            _authorService.AddPrintingEditionToAuthor(model.Author, model.PrintingEdition);
            return Ok();
        }
        public IActionResult GetAllEditions([FromQuery] int page = Constants.DEFAULTPAGE)
        {
            return Ok(_printingEditionService.GetPrintingEditionsFiltered(page: page, getRemoved: true));
        }
        public IActionResult EditPrintingEdition([FromBody] PrintingEditionModel printingEdition)
        {
            _printingEditionService.UpdatePrintingEdition(printingEdition);
            return Ok();
        }
        public IActionResult AddPrintingEdition([FromBody] PrintingEditionModel printingEdition)
        {
            _printingEditionService.AddPrintingEdition(printingEdition);
            return Ok();
        }
        public IActionResult DeletePrintingEdition([FromBody] PrintingEditionModel printingEdition)
        {
            _printingEditionService.DeletePrintingEdition(printingEdition);
            return Ok();
        }
    }
}
