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
using System.Threading.Tasks;
using System.Web;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Google.Apis.Auth;
using System.IO;
using System.Net.Mail;

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
        private readonly RNGCryptoServiceProvider _rngProvider;
        private readonly ICloudStorageProvider _cloudStorage;

        public UserService(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager,IOptions<UrlConfig> urlConfig, IMapper mapper,IJwtProvider jwtProvider, IEmailProvider emailProvider, IValidationProvider validationProvider, ICloudStorageProvider cloudStorage)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _validator = validationProvider;
            _urlConfig = urlConfig.Value;
            _email = emailProvider;
            _rngProvider = new RNGCryptoServiceProvider();
            _cloudStorage = cloudStorage;
        }
        public UsersViewModel GetViewModel(UsersViewModel model)
        {
            return new UsersViewModel
            {
                Users = GetUsers(model.GetBlocked, model.GetUnblocked, model.SearchString, model.Page),
                Page = model.Page,
                LastPage = GetLastPage(model.GetBlocked, model.GetUnblocked, model.SearchString),
                GetBlocked = model.GetBlocked,
                GetUnblocked = model.GetUnblocked,
                SearchString = string.IsNullOrWhiteSpace(model.SearchString) ? string.Empty : model.SearchString
            };
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
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.USERNOTFOUNDERROR);
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

        public async Task<TokensModel> GoogleLoginAsync(string idToken)
        {
            var tokenPayload = await DecodeJwtTokenAsync(idToken);
            string email = tokenPayload.Email;
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                var newUser = new UserModel
                {
                    FirstName = tokenPayload.GivenName,
                    LastName = tokenPayload.FamilyName,
                    Email = email,
                    UserName = email.Split(Constants.EMAILSEPARATOR)[0]
                };
                await GoogleRegisterAsync(newUser);
                user = await _userManager.FindByEmailAsync(email);
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var jwtResult = await _jwtProvider.GenerateTokenAsync(user.UserName, user.Id, userRoles);
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

        public async Task BanAsync(string userId)
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
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.USERNOTFOUNDERROR);
            }
            var user = _mapper.Map<UserModel>(dbUser);
            return user;
        }

        public async Task<UserModel> GetUserByUsernameAsync(string userName)
        {
            var dbUser = await _userManager.FindByNameAsync(userName);
            if (dbUser is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.USERNOTFOUNDERROR);
            }
            var user = _mapper.Map<UserModel>(dbUser);
            return user;
        }

        public List<UserModel> GetUsers(bool getBlocked, bool getUnblocked, string searchString, int page)
        {

            Expression<Func<UserEntity, bool>> filter = user => (getBlocked && user.IsRemoved == true)
            || (getUnblocked && user.IsRemoved == false);
            var dbUsers = _userManager.Users.Where(filter).ToList();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                dbUsers = dbUsers.Where(user => $"{user.FirstName} {user.LastName}".ToLower().Contains(searchString.ToLower())).ToList();
            }
            var users = _mapper.Map<List<UserModel>>(dbUsers);
            return users.Skip((page - Constants.DEFAULTPREVIOUSPAGEOFFSET) * Constants.USERPAGESIZE)
                .Take(Constants.USERPAGESIZE).ToList();
        }


        public int GetLastPage(bool getBlocked, bool getUnblocked, string searchString)
        {
            Expression<Func<UserEntity, bool>> filter = user => (getBlocked && user.IsRemoved == true)
            || (getUnblocked && user.IsRemoved == false);
            var dbUsers = _userManager.Users.Where(filter).ToList();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                dbUsers = dbUsers.Where(user => $"{user.FirstName} {user.LastName}".ToLower().Contains(searchString.ToLower())).ToList();
            }
            int lastPage = (int)Math.Ceiling(dbUsers.Count / (double)Constants.ORDERPAGESIZE);
            return lastPage;
        }

        public async Task RegisterAsync(UserModel user)
        {
            _validator.ValidateUser(user);
            if(user.ProfilePicture is not null)
            {
                string extension = Path.GetExtension(user.ProfilePicture.FileName);
                string fileName = GetFileName(user.UserName, extension);
                user.ProfilePictureURL = await _cloudStorage.UploadFileAsync(user.ProfilePicture, fileName);
                user.ProfilePictureStorageName = fileName;
            }
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
            await _email.SendEmailAsync(new MailAddress(newUser.Email),Constants.DEFAULTEMAILCONFIRMATION,string.Format(Constants.DEFAULTEMAILCONFIRMATIONBODY, callbackUrl));
        }

        public async Task<UserModel> UpdateAsync(UserModel user)
        {
            var dbUser = await _userManager.FindByIdAsync(user.Id);
            if (dbUser is null)
            {
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.USERNOTFOUNDERROR);
            }
            if (user.ProfilePicture is not null)
            {
                string extension = Path.GetExtension(user.ProfilePicture.FileName);
                string fileName = GetFileName(user.UserName, extension);
                user.ProfilePictureURL = await _cloudStorage.UploadFileAsync(user.ProfilePicture, fileName);
                user.ProfilePictureStorageName = fileName;
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
                throw new CustomApiException(HttpStatusCode.NotFound, Constants.USERNOTFOUNDERROR);
            }
            var result = await _userManager.ResetPasswordAsync(user, code, password);
            if (!result.Succeeded)
            {
                throw new CustomApiException(HttpStatusCode.Conflict, Constants.FAILEDTORESETPASSWORDERROR);
            }
            return;
        }

        private async Task<GoogleJsonWebSignature.Payload> DecodeJwtTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.INVALIDTOKENERROR);
            }
            var validPayload = await GoogleJsonWebSignature.ValidateAsync(token);
            return validPayload;
        }
        private async Task GoogleRegisterAsync(UserModel user)
        {
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
            user.Password = GetRandomString(Constants.DEFAULTPASSWORDLENGTH);
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
            await _userManager.ConfirmEmailAsync(newUser, code);
            await _email.SendEmailAsync(new System.Net.Mail.MailAddress(newUser.Email),Constants.DEFAULTPASSWORDGENERATED,string.Format(Constants.DEFAULTPASSWORDGENERATEDBODY, user.UserName, user.Password));
        }
        private string GetRandomString(int length)
        {
            char[] characterArray = Constants.ALPHANUMERICCHARACTERS.Distinct().ToArray();
            byte[] bytes = new byte[length * 8];
            _rngProvider.GetBytes(bytes);
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                ulong value = BitConverter.ToUInt64(bytes, i * 8);
                result[i] = characterArray[value % (uint)characterArray.Length];
            }
            return new string(result);
        }
        private string GetFileName(string username, string extension)
        {
            return $"{username}.{extension}";
        }

    }
}
