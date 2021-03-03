using EducationApp.BusinessLogicLayer.Models.Helpers;
using EducationApp.BusinessLogicLayer.Models.Requests;
using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.BusinessLogicLayer.Services.Interfaces;
using EducationApp.Shared.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            return Ok(Constants.DEFAULTEMAILCONFIRMATIONRESPONSE);

        }

        [HttpPost("Account/Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {
            var loginResult = await _userService.LoginAsync(model.User, model.RememberMe);
            return Ok(loginResult);

        }
        [HttpPost("Account/Logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout([FromBody] ConfirmEmailRequestModel model)
        {
            await _userService.LogoutAsync(model.UserId);
            return Ok();

        }

        [HttpPost("Account/ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequestModel model)
        {
            await _userService.ConfirmEmailAsync(model.UserId, model.Code);
            return Ok();
        }

        [HttpPost("Account/RefreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] TokenHelperModel model)
        {
            var loginResult = await _userService.RefreshTokenAsync(model.AccessToken, model.RefreshToken);
            return Ok(loginResult);
        }

        [HttpPost("Account/ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestModel model)
        {
            await _userService.ResetPasswordAsync(model.UserId, model.Code, model.Password);
            return Ok();
        }

        [HttpPost("Account/ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestModel model)
        {
            await _userService.ForgotPasswordAsync(model.UserName);
            return Ok(Constants.DEFAULTPASSWORDRESETRESPONSE);
        }

        [HttpGet("Account/GetUsers")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public IActionResult GetUsers()
        {
            return Ok(_userService.GetUsers());
        }

        [HttpGet("Account/GetUserInfo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,client")]
        public async Task<IActionResult> GetUserInfo([FromQuery] string userId)
        {
            return Ok(await _userService.GetUserByIdAsync(userId));
        }
        [HttpPost("Account/UpdateUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,client")]
        public async Task<IActionResult> UpdateUser([FromBody] UserModel user)
        {
            await _userService.UpdateAsync(user);
            return Ok();
        }
        [HttpPost("Account/BanUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> BanUser([FromBody] BanRequestModel model )
        {
            await _userService.BanAsync(model.User, model.Duration);
            return Ok();
        }
        [HttpPost("Account/UnbanUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> UnbanUser([FromBody] UserModel user)
        {
            await _userService.UnbanAsync(user);
            return Ok();
        }
        [HttpPost("Account/ChangePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestModel model)
        {
            await _userService.ChangePasswordAsync(model.User, model.CurrentPassword, model.NewPassword);
            return Ok();
        }
    }
}
