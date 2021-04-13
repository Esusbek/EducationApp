using EducationApp.BusinessLogicLayer.Models.Authors;
using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.BusinessLogicLayer.Models.ViewModels;
using EducationApp.BusinessLogicLayer.Services.Interfaces;
using EducationApp.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Threading.Tasks;

namespace EducationApp.PresentationLayer.Controllers
{
    [Authorize(Roles = "admin")]
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
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutByNameAsync(User.Identity.Name);
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> Profile()
        {
            var user = await _userService.GetUserByUsernameAsync(User.Identity.Name);
            return View(new ProfileViewModel { User = user });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromForm] UserModel user)
        {
            await _userService.UpdateAsync(user);
            return RedirectToAction("Profile");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOtherUser([FromForm] UserModel user)
        {
            await _userService.UpdateAsync(user);
            return RedirectToAction("Users");
        }

        [HttpGet]
        public IActionResult Users([FromQuery] UsersViewModel model)
        {
            return View(new UsersViewModel
            {
                Users = _userService.GetUsers(model.GetBlocked, model.GetUnblocked, model.SearchString, model.Page),
                Page = model.Page,
                LastPage = _userService.GetLastPage(model.GetBlocked, model.GetUnblocked, model.SearchString),
                GetBlocked = model.GetBlocked,
                GetUnblocked = model.GetUnblocked,
                SearchString = string.IsNullOrWhiteSpace(model.SearchString) ? "" : model.SearchString
            });
        }
        public async Task<IActionResult> Ban(string userId)
        {
            await _userService.BanAsync(userId);
            return Ok();
        }
        [HttpGet]
        public IActionResult PrintingEditions([FromQuery] PrintingEditionsViewModel model)
        {
            return View(new PrintingEditionsViewModel
            {
                PrintingEditions = _printingEditionService.GetPrintingEditionsAdmin(model.GetBook, model.GetNewspaper, model.GetJournal, model.SortBy, model.Ascending),
                Page = model.Page,
                LastPage = _printingEditionService.GetLastPage(model.GetBook, model.GetNewspaper, model.GetJournal),
                Ascending = model.Ascending,
                Authors = _authorService.GetAllAuthors(),
                GetBook = model.GetBook,
                GetJournal = model.GetJournal,
                GetNewspaper = model.GetNewspaper,
                SortBy = model.SortBy
            });
        }
        [HttpGet]
        public IActionResult Orders([FromQuery] OrdersViewModel model)
        {
            return View(new OrdersViewModel
            {
                Orders = _orderService.GetAllOrders(model.GetPaid, model.GetUnpaid, model.SortBy, model.Ascending, model.Page),
                Page = model.Page,
                LastPage = _orderService.GetLastPage(model.GetPaid, model.GetUnpaid),
                GetUnpaid = model.GetUnpaid,
                GetPaid = model.GetPaid,
                SortBy = model.SortBy,
                Ascending = model.Ascending
            });
        }
        [HttpGet]
        public IActionResult Authors([FromQuery] AuthorsViewModel model)
        {
            return View(new AuthorsViewModel
            {
                Authors = _authorService.GetAuthorsFiltered(null, model.SortBy, model.Ascending, page: model.Page),
                Page = model.Page,
                LastPage = _authorService.GetLastPage(),
                SortBy = model.SortBy,
                Ascending = model.Ascending
            });
        }
        [HttpPost]
        public IActionResult EditAuthor(AuthorModel author)
        {
            if (ModelState.IsValid)
            {
                _authorService.UpdateAuthor(author);
                return RedirectToAction("Authors");
            }
            return View("Error", Constants.VALIDATIONERROR);
        }
        public IActionResult AddAuthor(AuthorModel author)
        {
            if (ModelState.IsValid)
            {
                _authorService.AddAuthor(author);
                return RedirectToAction("Authors");
            }
            return View("Error", Constants.VALIDATIONERROR);
        }
        public IActionResult DeleteAuthor(AuthorModel author)
        {
            if (ModelState.IsValid)
            {
                _authorService.DeleteAuthor(author);
                return RedirectToAction("Authors");
            }
            return View("Error", Constants.VALIDATIONERROR);
        }
        [HttpPost]
        public IActionResult EditEdition([FromForm] PrintingEditionModel printingEdition)
        {
            if (!ModelState.IsValid)
            {
                return View("Error", Constants.VALIDATIONERROR);
            }
            _printingEditionService.UpdatePrintingEdition(printingEdition);
            return RedirectToAction("PrintingEditions");
        }
        public IActionResult AddEdition([FromForm] PrintingEditionModel printingEdition)
        {
            if (!ModelState.IsValid)
            {
                _printingEditionService.AddPrintingEdition(printingEdition);
                return RedirectToAction("PrintingEditions");
            }
            return View("Error", Constants.VALIDATIONERROR);
        }
        public IActionResult DeleteEdition([FromForm] PrintingEditionModel printingEdition)
        {
            if (ModelState.IsValid)
            {
                _printingEditionService.DeletePrintingEdition(printingEdition);
                return RedirectToAction("PrintingEditions");
            }
            return View("Error", Constants.VALIDATIONERROR);
        }
    }
}
