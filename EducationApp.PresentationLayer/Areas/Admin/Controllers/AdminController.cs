using EducationApp.BusinessLogicLayer.Models.Authors;
using EducationApp.BusinessLogicLayer.Models.PrintingEditions;
using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.BusinessLogicLayer.Models.ViewModels;
using EducationApp.BusinessLogicLayer.Services.Interfaces;
using EducationApp.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return View(_userService.GetViewModel(model));
        }
        public async Task<IActionResult> Ban(string userId)
        {
            await _userService.BanAsync(userId);
            return Ok();
        }
        [HttpGet]
        public IActionResult PrintingEditions([FromQuery] PrintingEditionsViewModel model)
        {
            return View(_printingEditionService.GetViewModel(model));
        }
        [HttpGet]
        public IActionResult Orders([FromQuery] OrdersViewModel model)
        {
            return View(_orderService.GetViewModel(model));
        }
        [HttpGet]
        public IActionResult Authors([FromQuery] AuthorsViewModel model)
        {
            return View(_authorService.GetViewModel(model));
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
