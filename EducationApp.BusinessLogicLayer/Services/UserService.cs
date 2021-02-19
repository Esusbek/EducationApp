using AutoMapper;
using EducationApp.BusinessLogicLayer.Models.Helpers;
using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.BusinessLogicLayer.Providers.Interfaces;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.Shared.Configs;
using EducationApp.Shared.Constants;
using EducationApp.Shared.Enums;
using EducationApp.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace EducationApp.BusinessLogicLayer.Services
{
    public class UserService : Interfaces.IUserService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserValidationProvider _validator;
        private readonly IEmailProvider _email;
        private readonly UrlConfig _urlConfig;
        private readonly IJwtProvider _jwtProvider;

        public UserService(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager,
            IOptions<UrlConfig> urlConfig, IMapper mapper,
            IJwtProvider jwtProvider, IEmailProvider emailProvider, IUserValidationProvider userValidationProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _validator = userValidationProvider;
            _urlConfig = urlConfig.Value;
            _email = emailProvider;
        }
        public async Task<TokenHelperModel> LoginAsync(UserModel user, bool rememberMe)
        {
            if (string.IsNullOrWhiteSpace(user.Password) || string.IsNullOrWhiteSpace(user.UserName))
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.INCORRECTINPUTERROR);
            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, rememberMe, false);
            if (!result.Succeeded)
            {
                throw new CustomApiException(HttpStatusCode.Unauthorized, Constants.FAILEDLOGINERROR);
            }
            var dbUser = await _userManager.FindByNameAsync(user.UserName);
            var userRoles = await _userManager.GetRolesAsync(dbUser);
            var jwtResult = await _jwtProvider.GenerateTokenAsync(dbUser.UserName, dbUser.Id, userRoles);
            return jwtResult;
        }

        public async Task LogoutAsync(string userId)
        {
            await _signInManager.SignOutAsync();
            await _jwtProvider.RemoveRefreshTokenAsync(await _userManager.FindByIdAsync(userId));
        }

        public async Task<TokenHelperModel> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken) || string.IsNullOrWhiteSpace(accessToken))
            {
                throw new CustomApiException(HttpStatusCode.Unauthorized, Constants.UNATHORIZEDERROR);
            }
            var jwtResult = await _jwtProvider.RefreshAsync(refreshToken, accessToken);
            return jwtResult;
        }

        public async Task<bool> AuthorizationAsync(UserEntity user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }

        public async void BanAsync(UserModel user, int duration)
        {
            var dbUser = _mapper.Map<UserEntity>(user);
            dbUser.Id = user.Id.ToString();
            await _userManager.SetLockoutEnabledAsync(dbUser, true);
            await _userManager.SetLockoutEndDateAsync(dbUser, DateTime.Today.AddDays(duration));
        }

        public async Task<UserEntity> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.USERNOTFOUNDERROR);
            }
            return user;
        }
        public async Task<UserEntity> GetUserByUsernameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.USERNOTFOUNDERROR);
            }
            return user;
        }

        public List<UserModel> GetUsers()
        {
            var dbUsers = _userManager.Users.ToList();
            var users = new List<UserModel>();
            foreach (var user in dbUsers)
            {
                users.Add(_mapper.Map<UserModel>(user));
            }
            return users;
        }

        public async Task RegisterAsync(UserModel user)
        {
            _validator.Validate(user);
            var newUser = _mapper.Map<UserEntity>(user);

            var result = await _userManager.CreateAsync(newUser, user.Password);
            if (!result.Succeeded)
            {
                throw new CustomApiException(HttpStatusCode.Conflict, Constants.FAILEDTOCREATEUSERERROR);
            }
            result = await _userManager.AddToRoleAsync(newUser, Enums.UserRolesType.Client.ToString("g"));
            if (!result.Succeeded)
            {
                await _userManager.DeleteAsync(newUser);
                throw new CustomApiException(HttpStatusCode.Conflict, Constants.FAILEDTOCREATEUSERERROR);
            }
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var uriBuilder = new UriBuilder
            {
                Scheme = _urlConfig.Scheme,
                Port = _urlConfig.Port,
                Host = _urlConfig.Host,
                Path = Constants.CONFIRMEMAILPATH
            };
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["userId"] = newUser.Id;
            query["code"] = code;
            uriBuilder.Query = query.ToString();
            var callbackUrl = uriBuilder.ToString();
            await _email.SendEmailAsync(new System.Net.Mail.MailAddress(newUser.Email),
                Constants.DEFAULTEMAILCONFIRMATION,
                string.Format(Constants.DEFAULTEMAILCONFIRMATIONBODY, callbackUrl));
        }

        public async Task UpdateAsync(UserModel user)
        {
            var dbUser = _mapper.Map<UserEntity>(user);
            dbUser.Id = user.Id.ToString();
            await _userManager.UpdateAsync(dbUser);
        }

        public async Task ConfirmEmailAsync(string id, string code)
        {
            if (id is null || string.IsNullOrWhiteSpace(code))
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.INCORRECTINPUTERROR);
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.USERNOTFOUNDERROR);
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.INVALIDTOKENERROR);
            }

        }

        public async Task ForgotPasswordAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.USERNOTFOUNDERROR);
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var uriBuilder = new UriBuilder
            {
                Scheme = _urlConfig.Scheme,
                Port = _urlConfig.Port,
                Host = _urlConfig.Host,
                Path = Constants.RESETPASSWORDPATH
            };
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["userId"] = user.Id;
            query["code"] = code;
            uriBuilder.Query = query.ToString();
            var callbackUrl = uriBuilder.ToString();
            await _email.SendEmailAsync(new System.Net.Mail.MailAddress(user.Email),
                Constants.DEFAULTPASSWORDRESET,
                string.Format(Constants.DEFAULTPASSWORDRESETBODY, callbackUrl));
        }

        public async Task ResetPasswordAsync(string userId, string code, string password)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.USERNOTFOUNDERROR);
            }
            var result = await _userManager.ResetPasswordAsync(user, code, password);
            if (!result.Succeeded)
            {
                throw new CustomApiException(HttpStatusCode.Conflict, Constants.FAILEDTORESETPASSWORDERROR);
            }
            return;
        }
    }
}
