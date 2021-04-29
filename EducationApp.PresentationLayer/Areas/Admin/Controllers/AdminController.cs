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
            await _userService.LoginAsync(model);
            return RedirectToAction("Profile");
        }
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutByNameAsync(User.Identity.Name);
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> Profile()
        {
            var model = await _userService.GetProfileViewModelAsync(User.Identity.Name);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromForm] UserModel user)
        {
            await _userService.UpdateAsync(user);
            return RedirectToAction("Profile");
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromForm] ChangeAdminPasswordRequestModel model)
        {
            await _userService.ChangePasswordAsync(new UserModel { Id=model.Id }, model.CurrentPassword, model.NewPassword);
            return Ok();
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
            var newModel = _userService.GetViewModel(model);
            return View(newModel);
        }
        public async Task<IActionResult> Ban(string userId)
        {
            await _userService.RemoveAsync(userId);
            return Ok();
        }
        [HttpGet]
        public IActionResult PrintingEditions([FromQuery] PrintingEditionsViewModel model)
        {
            var newModel = _printingEditionService.GetViewModel(model);
            return View(newModel);
        }
        [HttpGet]
        public IActionResult Orders([FromQuery] OrdersViewModel model)
        {
            var newModel = _orderService.GetViewModel(model);
            return View(newModel);
        }
        [HttpGet]
        public IActionResult Authors([FromQuery] AuthorsViewModel model)
        {
            var newModel = _authorService.GetViewModel(model);
            return View(newModel);
        }
        [HttpPost]
        public IActionResult EditAuthor(AuthorModel author)
        {
            if (!ModelState.IsValid)
            {
                return View("Error", Constants.VALIDATIONERROR);
            }
            _authorService.UpdateAuthor(author);
            return RedirectToAction("Authors");
        }
        public IActionResult AddAuthor(AuthorModel author)
        {
            if (!ModelState.IsValid)
            {
                return View("Error", Constants.VALIDATIONERROR);
            }
            _authorService.AddAuthor(author);
            return RedirectToAction("Authors");
        }
        public IActionResult DeleteAuthor(AuthorModel author)
        {
            if (!ModelState.IsValid)
            {
                return View("Error", Constants.VALIDATIONERROR);
            }
            _authorService.DeleteAuthor(author);
            return RedirectToAction("Authors");
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
            if (!!ModelState.IsValid)
            {
                return View("Error", Constants.VALIDATIONERROR);
            }
            _printingEditionService.AddPrintingEdition(printingEdition);
            return RedirectToAction("PrintingEditions");
        }
        public IActionResult DeleteEdition([FromForm] PrintingEditionModel printingEdition)
        {
            if (!ModelState.IsValid)
            {
                return View("Error", Constants.VALIDATIONERROR);
            }
            _printingEditionService.DeletePrintingEdition(printingEdition);
            return RedirectToAction("PrintingEditions");
        }
    }
}
