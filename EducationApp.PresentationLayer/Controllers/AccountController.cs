using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EducationApp.PresentationLayer.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserModel user)
        {
            await _userService.RegisterAsync(user);
            return Content("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");

        }

        [HttpPost]
        public async Task<IActionResult> Login(UserModel user, bool rememberMe)
        {
            await _userService.LoginAsync(user, rememberMe);
            return Content("LoginSuccessful");

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            await _userService.ConfirmEmailAsync(userId, code);
            return RedirectToAction("Index", "Account");
        }


    }
}
