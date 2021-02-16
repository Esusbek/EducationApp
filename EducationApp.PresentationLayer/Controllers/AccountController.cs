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
            var loginResult = await _userService.LoginAsync(user, rememberMe);
            return Ok(loginResult);

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            await _userService.ConfirmEmailAsync(userId, code);
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [Authorize]
        public IActionResult RefreshToken(string accessToken, string refreshToken)
        {
            var loginResult = _userService.RefreshToken(accessToken, refreshToken);
            return Ok(loginResult);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string userId, string code, string password)
        {
            await _userService.ResetPasswordAsync(userId, code, password);
            return RedirectToAction("Index", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(string userName)
        {
            await _userService.ForgotPasswordAsync(userName);
            return RedirectToAction("Index", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetUsers()
        {
            return Ok(_userService.GetUsers());
        }

    }
}
