using AutoMapper;
using EducationApp.BusinessLogicLayer.Models;
using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.BusinessLogicLayer.Models.ViewModels;
using EducationApp.BusinessLogicLayer.Providers.Interfaces;
using EducationApp.BusinessLogicLayer.Services.Interfaces;
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
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace EducationApp.BusinessLogicLayer.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly IMapper _mapper;
        private readonly IValidationProvider _validator;
        private readonly IEmailProvider _email;
        private readonly UrlConfig _urlConfig;
        private readonly IJwtProvider _jwtProvider;

        public UserService(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager,
            IOptions<UrlConfig> urlConfig, IMapper mapper,
            IJwtProvider jwtProvider, IEmailProvider emailProvider, IValidationProvider validationProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _validator = validationProvider;
            _urlConfig = urlConfig.Value;
            _email = emailProvider;
        }
        public UsersViewModel GetViewModel(UsersViewModel model)
        {
            return new UsersViewModel
            {
                Users = GetUsers(model.IsBlocked, model.IsUnblocked, model.SearchString, model.Page),
                Page = model.Page,
                PageCount = GetPageCount(model.IsBlocked, model.IsUnblocked, model.SearchString),
                IsBlocked = model.IsBlocked,
                IsUnblocked = model.IsUnblocked,
                SearchString = string.IsNullOrWhiteSpace(model.SearchString) ? string.Empty : model.SearchString
            };
        }
        public async Task<ProfileViewModel> GetProfileViewModelAsync(string username)
        {
            var user = await GetUserByUsernameAsync(username);
            return new ProfileViewModel { User = user };
        }
        public async Task<TokensModel> LoginAsync(LoginViewModel model, bool rememberMe = true)
        {
            var user = new UserModel
            {
                Password = model.Password,
                UserName = model.UserName
            };
            var jwtResult = await LoginAsync(user, rememberMe);
            return jwtResult;
        }
        public async Task<TokensModel> LoginAsync(UserModel user, bool rememberMe)
        {
            if (string.IsNullOrWhiteSpace(user.Password) || string.IsNullOrWhiteSpace(user.UserName))
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.INCORRECTINPUTERROR);
            }
            var dbUser = await _userManager.FindByNameAsync(user.UserName);
            if (dbUser is null)
            {
                throw new CustomApiException(HttpStatusCode.BadRequest, Constants.USERNOTFOUNDERROR);
            }
            if (dbUser.IsRemoved)
            {
                throw new CustomApiException(HttpStatusCode.Forbidden, Constants.USERBANNED);
            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, rememberMe, false);
            if (!result.Succeeded)
            {
                throw new CustomApiException(HttpStatusCode.Forbidden, Constants.FAILEDLOGINERROR);
            }
            var userRoles = await _userManager.GetRolesAsync(dbUser);
            var jwtResult = await _jwtProvider.GenerateTokenAsync(dbUser.UserName, dbUser.Id, userRoles);
            return jwtResult;
        }

        public async Task LogoutAsync(string userId)
        {
            await _signInManager.SignOutAsync();
            var dbUser = await _userManager.FindByIdAsync(userId);
            if (dbUser is not null)
            {
                await _jwtProvider.RemoveRefreshTokenAsync(dbUser);
            }
        }

        public async Task LogoutByNameAsync(string userName)
        {
            await _signInManager.SignOutAsync();
            var dbUser = await _userManager.FindByNameAsync(userName);
            if (dbUser is not null)
            {
                await _jwtProvider.RemoveRefreshTokenAsync(dbUser);
            }
        }

        public async Task<TokensModel> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken) || string.IsNullOrWhiteSpace(accessToken))
            {
                throw new CustomApiException(HttpStatusCode.Forbidden, Constants.UNATHORIZEDERROR);
            }
            var jwtResult = await _jwtProvider.RefreshAsync(refreshToken, accessToken);
            return jwtResult;
        }

        public async Task RemoveAsync(string userId)
        {
            var dbUser = await _userManager.FindByIdAsync(userId);
            dbUser.IsRemoved = !dbUser.IsRemoved;
            await _userManager.UpdateAsync(dbUser);
        }

        public async Task ChangePasswordAsync(UserModel user, string currentPassword, string newPassword)
        {
            var dbUser = _mapper.Map<UserEntity>(user);
            dbUser.Id = user.Id;
            var result = await _userManager.ChangePasswordAsync(dbUser, currentPassword, newPassword);
            if (!result.Succeeded)
            {
                throw new CustomApiException(HttpStatusCode.Conflict, Constants.PASSWORDCHANGEFAILEDERROR);
            }
        }

        public async Task<UserModel> GetUserByIdAsync(string id)
        {
            var dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser is null)
            {
                throw new CustomApiException(HttpStatusCode.BadRequest, Constants.USERNOTFOUNDERROR);
            }
            var user = _mapper.Map<UserModel>(dbUser);
            return user;
        }
        public async Task<UserModel> GetUserByUsernameAsync(string userName)
        {
            var dbUser = await _userManager.FindByNameAsync(userName);
            if (dbUser is null)
            {
                throw new CustomApiException(HttpStatusCode.BadRequest, Constants.USERNOTFOUNDERROR);
            }
            var user = _mapper.Map<UserModel>(dbUser);
            return user;
        }

        public List<UserModel> GetUsers(bool isBlocked, bool isUnblocked, string searchString, int page)
        {

            Expression<Func<UserEntity, bool>> filter = user => (isBlocked && user.IsRemoved)
            || (isUnblocked && !user.IsRemoved);
            var dbUsers = _userManager.Users.Where(filter).ToList();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                dbUsers = dbUsers.Where(user => $"{user.FirstName} {user.LastName}".ToLower().Contains(searchString.ToLower())).ToList();
            }
            var users = _mapper.Map<List<UserModel>>(dbUsers);
            return users.Skip((page - Constants.DEFAULTPREVIOUSPAGEOFFSET) * Constants.USERPAGESIZE)
                .Take(Constants.USERPAGESIZE).ToList();
        }


        public int GetPageCount(bool isBlocked, bool isUnblocked, string searchString)
        {
            Expression<Func<UserEntity, bool>> filter = user => (isBlocked && user.IsRemoved == true)
            || (isUnblocked && user.IsRemoved == false);
            var dbUsers = _userManager.Users.Where(filter);
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                dbUsers = dbUsers.Where(user => $"{user.FirstName} {user.LastName}".ToLower().Contains(searchString.ToLower()));
            }
            int pageCount = (int)Math.Ceiling(dbUsers.Count() / (double)Constants.ORDERPAGESIZE);
            return pageCount;
        }

        public async Task RegisterAsync(UserModel user)
        {
            _validator.ValidateUser(user);
            var newUser = _mapper.Map<UserEntity>(user);
            var dbUser = await _userManager.FindByNameAsync(user.UserName);
            if (dbUser is not null)
            {
                throw new CustomApiException(HttpStatusCode.Conflict, Constants.USERNAMETAKENERROR);
            }
            dbUser = await _userManager.FindByEmailAsync(user.Email);
            if (dbUser is not null)
            {
                throw new CustomApiException(HttpStatusCode.Conflict, Constants.EMAILTAKENERROR);
            }
            var result = await _userManager.CreateAsync(newUser, user.Password);
            if (!result.Succeeded)
            {
                throw new CustomApiException(HttpStatusCode.Conflict, Constants.FAILEDTOCREATEUSERERROR);
            }
            result = await _userManager.AddToRoleAsync(newUser, Enums.UserRolesType.Client.ToString());
            if (!result.Succeeded)
            {
                await _userManager.DeleteAsync(newUser);
                throw new CustomApiException(HttpStatusCode.Conflict, Constants.FAILEDTOCREATEUSERERROR);
            }
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var uriBuilder = new UriBuilder
            {
                Scheme = _urlConfig.Scheme,
                Port = _urlConfig.Port,
                Host = _urlConfig.Host,
                Path = Constants.CONFIRMEMAILPATH
            };
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[Constants.USERIDKEY] = newUser.Id;
            query[Constants.CODEKEY] = code;
            uriBuilder.Query = query.ToString();
            string callbackUrl = uriBuilder.ToString();
            await _email.SendEmailAsync(new MailAddress(newUser.Email),
                Constants.DEFAULTEMAILCONFIRMATION,
                string.Format(Constants.DEFAULTEMAILCONFIRMATIONBODY, callbackUrl));
        }

        public async Task<UserModel> UpdateAsync(UserModel user)
        {
            var dbUser = await _userManager.FindByIdAsync(user.Id);
            if (dbUser is null)
            {
                throw new CustomApiException(HttpStatusCode.BadRequest, Constants.USERNOTFOUNDERROR);
            }
            _mapper.Map(user, dbUser);
            await _userManager.UpdateAsync(dbUser);
            var updatedUser = _mapper.Map<UserModel>(dbUser);
            return updatedUser;
        }

        public async Task ConfirmEmailAsync(string id, string code)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(code))
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.INCORRECTINPUTERROR);
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                throw new CustomApiException(HttpStatusCode.BadRequest, Constants.USERNOTFOUNDERROR);
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
                throw new CustomApiException(HttpStatusCode.BadRequest, Constants.USERNOTFOUNDERROR);
            }
            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var uriBuilder = new UriBuilder
            {
                Scheme = _urlConfig.Scheme,
                Port = _urlConfig.Port,
                Host = _urlConfig.Host,
                Path = Constants.RESETPASSWORDPATH
            };
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[Constants.USERIDKEY] = user.Id;
            query[Constants.CODEKEY] = code;
            uriBuilder.Query = query.ToString();
            string callbackUrl = uriBuilder.ToString();
            await _email.SendEmailAsync(new MailAddress(user.Email),
                Constants.DEFAULTPASSWORDRESET,
                string.Format(Constants.DEFAULTPASSWORDRESETBODY, callbackUrl));
        }

        public async Task ResetPasswordAsync(string userId, string code, string password)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(password))
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.INCORRECTINPUTERROR);
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                throw new CustomApiException(HttpStatusCode.BadRequest, Constants.USERNOTFOUNDERROR);
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
