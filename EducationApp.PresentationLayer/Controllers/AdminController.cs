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
        public IActionResult Users([FromQuery] UsersViewModel model, [FromQuery] int page = Constants.DEFAULTPAGE)
        {
            return View(new UsersViewModel
            {
                Users = _userService.GetUsers(model.GetBlocked, model.GetUnblocked, model.SearchString, page),
                CurrentPage = page,
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
        public IActionResult PrintingEditions([FromQuery] PrintingEditionsViewModel model, [FromQuery] int page = Constants.DEFAULTPAGE)
        {
            return View(new PrintingEditionsViewModel
            {
                PrintingEditions = _printingEditionService.GetPrintingEditionsAdmin(model.GetBook, model.GetNewspaper, model.GetJournal, model.SortBy, model.Ascending),
                CurrentPage = page,
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
        public IActionResult Orders([FromQuery] OrdersViewModel model, [FromQuery] int page = Constants.DEFAULTPAGE)
        {
            return View(new OrdersViewModel
            {
                Orders = _orderService.GetAllOrders(model.GetPaid, model.GetUnpaid, model.SortBy, model.Ascending, page),
                CurrentPage = page,
                LastPage = _orderService.GetLastPage(model.GetPaid, model.GetUnpaid),
                GetUnpaid = model.GetUnpaid,
                GetPaid = model.GetPaid,
                SortBy = model.SortBy,
                Ascending = model.Ascending
            });
        }
        [HttpGet]
        public IActionResult Authors([FromQuery] AuthorsViewModel model, [FromQuery] int page = Constants.DEFAULTPAGE)
        {
            return View(new AuthorsViewModel
            {
                Authors = _authorService.GetAuthorsFiltered(null, model.SortBy, model.Ascending, page: page),
                CurrentPage = page,
                LastPage = _authorService.GetLastPage(),
                SortBy = model.SortBy,
                Ascending = model.Ascending
            });
        }
        [HttpPost]
        public IActionResult EditAuthor(AuthorModel author)
        {
            _authorService.UpdateAuthor(author);
            return RedirectToAction("Authors");
        }
        public IActionResult AddAuthor(AuthorModel author)
        {
            _authorService.AddAuthor(author);
            return RedirectToAction("Authors");
        }
        public IActionResult DeleteAuthor(AuthorModel author)
        {
            _authorService.DeleteAuthor(author);
            return RedirectToAction("Authors");
        }
        [HttpPost]
        public IActionResult EditEdition([FromForm] PrintingEditionModel printingEdition, [FromForm] string price)
        {
            decimal priceDecimal = decimal.Parse(price, CultureInfo.InvariantCulture);
            printingEdition.Price = priceDecimal;
            _printingEditionService.UpdatePrintingEdition(printingEdition);
            return RedirectToAction("PrintingEditions");
        }
        public IActionResult AddEdition([FromForm] PrintingEditionModel printingEdition)
        {
            _printingEditionService.AddPrintingEdition(printingEdition);
            return RedirectToAction("PrintingEditions");
        }
        public IActionResult DeleteEdition([FromForm] PrintingEditionModel printingEdition)
        {
            _printingEditionService.DeletePrintingEdition(printingEdition);
            return RedirectToAction("PrintingEditions");
        }
    }
}
