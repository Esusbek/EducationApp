using AutoMapper;
using EducationApp.BusinessLogicLayer.Exceptions;
using EducationApp.BusinessLogicLayer.Helpers;
using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.Shared.Configs;
using EducationApp.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
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
        private readonly CustomUserValidator<UserModel> _validator;
        private readonly EmailHelper _email;
        private readonly UrlConfig _urlConfig;

        public UserService(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, IOptions<SmtpConfig> smtpConfig, IOptions<UrlConfig> urlConfig)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            var cfg = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserModel, UserEntity>();
            });
            _mapper = cfg.CreateMapper();
            _validator = new CustomUserValidator<UserModel>();
            _urlConfig = urlConfig.Value;
            _email = new EmailHelper(smtpConfig.Value);
        }
        public async Task<UserModel> LoginAsync(UserModel user, bool rememberMe)
        {
            if (user.Password != null && user.UserName != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, rememberMe, false);
                if (!result.Succeeded)
                {
                    throw new CustomApiException(HttpStatusCode.Unauthorized, Constants.Errors.FailedLogin);
                }
                var dbUser = await _userManager.FindByNameAsync(user.UserName);
                var authUser = _mapper.Map<UserModel>(dbUser);
                return authUser;
            }
            throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.Errors.IncorrectInput);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
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

        public async Task<UserEntity> GetUserAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public UserModel GetUsers()
        {

            _userManager.Users.ToList();
            return null; //model
        }

        public async void AddRoleAsync(UserEntity user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
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
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var uriBuilder = new UriBuilder();
            uriBuilder.Scheme = _urlConfig.Scheme;
            uriBuilder.Port = _urlConfig.Port;
            uriBuilder.Host = _urlConfig.Host;
            uriBuilder.Path = Constants.Urls.ConfirmEmailPath;
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["userId"] = newUser.Id;
            query["code"] = code;
            uriBuilder.Query = query.ToString();
            var callbackUrl = uriBuilder.ToString();
            await _email.SendEmailAsync(new System.Net.Mail.MailAddress(newUser.Email),
                "Email confirmation",
                $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");
        }

        public async void UpdateAsync(UserModel user)
        {

        }

        public async Task ConfirmEmailAsync(string id, string code)
        {
            if (id == null || code == null)
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.Errors.IncorrectInput);
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.Errors.UserNotFound);
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.Errors.InvalidToken);
            }

        }
    }
}
