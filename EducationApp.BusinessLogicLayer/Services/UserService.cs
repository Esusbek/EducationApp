using AutoMapper;
using EducationApp.BusinessLogicLayer.Helpers;
using EducationApp.BusinessLogicLayer.Helpers.Interfaces;
using EducationApp.BusinessLogicLayer.Models.Helpers;
using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.Shared.Configs;
using EducationApp.Shared.Constants;
using EducationApp.Shared.Enums;
using EducationApp.Shared.Exceptions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace EducationApp.BusinessLogicLayer.Services
{
    public class UserService : Interfaces.IUserService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly IMapper _mapper;
        private readonly CustomUserValidator<UserModel> _validator;
        private readonly EmailProvider _email;
        private readonly UrlConfig _urlConfig;
        private readonly IJwtProvider _jwtHelper;

        public UserService(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager,
            IOptions<SmtpConfig> smtpConfig, IOptions<UrlConfig> urlConfig, IMapper mapper, IJwtProvider jwtHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _jwtHelper = jwtHelper;
            _validator = new CustomUserValidator<UserModel>();
            _urlConfig = urlConfig.Value;
            _email = new EmailProvider(smtpConfig.Value);
        }
        public async Task<LoginResult> LoginAsync(UserModel user, bool rememberMe)
        {
            if (string.IsNullOrWhiteSpace(user.Password) || string.IsNullOrWhiteSpace(user.UserName))
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.Errors.IncorrectInput);
            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, rememberMe, false);
            if (!result.Succeeded)
            {
                throw new CustomApiException(HttpStatusCode.Unauthorized, Constants.Errors.FailedLogin);
            }
            var dbUser = await _userManager.FindByNameAsync(user.UserName);
            var userRoles = await _userManager.GetRolesAsync(dbUser);
            var claims = new List<Claim>();
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Name, dbUser.UserName));
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var jwtResult = _jwtHelper.GenerateToken(dbUser.UserName, claims);
            return new LoginResult
            {
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            };
        }

        public async Task LogoutAsync(string userId)
        {
            await _signInManager.SignOutAsync();
            _jwtHelper.RemoveRefreshTokenByUserName((await _userManager.FindByIdAsync(userId)).UserName);
        }

        public LoginResult RefreshToken(string accessToken, string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new CustomApiException(HttpStatusCode.Unauthorized, Constants.Errors.Unathorized);
            }
            var jwtResult = _jwtHelper.Refresh(refreshToken, accessToken);
            return new LoginResult
            {
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            };
        }

        public async Task<bool> AuthorizationAsync(UserEntity user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }

        public async void BanAsync(UserModel user, int duration)
        {
            var dbUser = _mapper.Map<UserEntity>(user);
            await _userManager.SetLockoutEnabledAsync(dbUser, true);
            await _userManager.SetLockoutEndDateAsync(dbUser, DateTime.Today.AddDays(duration));
        }

        public async Task<UserEntity> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.Errors.UserNotFound);
            }
            return user;
        }
        public async Task<UserEntity> GetUserByUsernameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.Errors.UserNotFound);
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
            var isValid = await _validator.ValidateAsync(null, user);
            if (!isValid.Succeeded)
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.Errors.IncorrectInput);
            }
            var newUser = _mapper.Map<UserEntity>(user);

            var result = await _userManager.CreateAsync(newUser, user.Password);
            if (!result.Succeeded)
            {
                throw new CustomApiException(HttpStatusCode.Conflict, Constants.Errors.FailedToCreate);
            }
            result = await _userManager.AddToRoleAsync(newUser, GetRoleName(Enums.User.Roles.Client));
            if (!result.Succeeded)
            {
                await _userManager.DeleteAsync(newUser);
                throw new CustomApiException(HttpStatusCode.Conflict, Constants.Errors.FailedToCreate);
            }
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var uriBuilder = new UriBuilder
            {
                Scheme = _urlConfig.Scheme,
                Port = _urlConfig.Port,
                Host = _urlConfig.Host,
                Path = Constants.Urls.ConfirmEmailPath
            };
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["userId"] = newUser.Id;
            query["code"] = code;
            uriBuilder.Query = query.ToString();
            var callbackUrl = uriBuilder.ToString();
            await _email.SendEmailAsync(new System.Net.Mail.MailAddress(newUser.Email),
                "Email confirmation",
                $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");
        }

        public async Task UpdateAsync(UserModel user)
        {
            var dbUser = _mapper.Map<UserEntity>(user);
            await _userManager.UpdateAsync(dbUser);
        }

        public async Task ConfirmEmailAsync(string id, string code)
        {
            if (id is null || string.IsNullOrWhiteSpace(code))
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.Errors.IncorrectInput);
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.Errors.UserNotFound);
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.Errors.InvalidToken);
            }

        }

        public async Task ForgotPasswordAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.Errors.UserNotFound);
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var uriBuilder = new UriBuilder
            {
                Scheme = _urlConfig.Scheme,
                Port = _urlConfig.Port,
                Host = _urlConfig.Host,
                Path = Constants.Urls.ResetPasswordPath
            };
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["userId"] = user.Id;
            query["code"] = code;
            uriBuilder.Query = query.ToString();
            var callbackUrl = uriBuilder.ToString();
            await _email.SendEmailAsync(new System.Net.Mail.MailAddress(user.Email),
                "Password reset request",
                $"Произведите сброс пароля, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");
        }

        public async Task ResetPasswordAsync(string userId, string code, string password)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.Errors.UserNotFound);
            }
            var result = await _userManager.ResetPasswordAsync(user, code, password);
            if (!result.Succeeded)
            {
                throw new CustomApiException(HttpStatusCode.Conflict, Constants.Errors.FailedToReset);
            }
            return;
        }
        private string GetRoleName(Enums.User.Roles role)
        {
            if ((int)role == 1)
            {
                return "client";
            }
            return "admin";
        }
    }
}
