using EducationApp.BusinessLogicLayer.Models.Helpers;
using EducationApp.BusinessLogicLayer.Models.Requests;
using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.BusinessLogicLayer.Services.Interfaces;
using EducationApp.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EducationApp.PresentationLayer.Controllers
{
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Account/Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserModel user)
        {
            await _userService.RegisterAsync(user);
            return Content(Constants.DEFAULTEMAILCONFIRMATIONRESPONSE);

        }

        [HttpPost("Account/Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {
            var loginResult = await _userService.LoginAsync(model.User, model.RememberMe);
            return Ok(loginResult);

        }

        [HttpGet("Account/ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequestModel model)
        {
            await _userService.ConfirmEmailAsync(model.UserId, model.Code);
            return RedirectToAction("Login", "Account");
        }

        [HttpPost("Account/RefreshToken")]
        [Authorize(Roles = "admin,client")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenHelperModel model)
        {
            var loginResult = await _userService.RefreshTokenAsync(model.AccessToken, model.RefreshToken);
            return Ok(loginResult);
        }

        [HttpGet("Account/ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestModel model)
        {
            await _userService.ResetPasswordAsync(model.UserId, model.Code, model.Password);
            return RedirectToAction("Login", "Account");
        }

        [HttpPost("Account/ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] string userName)
        {
            await _userService.ForgotPasswordAsync(userName);
            return Content(Constants.DEFAULTPASSWORDRESETRESPONSE);
        }

        [HttpGet("Account/GetUsers")]
        [Authorize(Roles = "admin")]
        public IActionResult GetUsers()
        {
            return Ok(_userService.GetUsers());
        }

    }
}
