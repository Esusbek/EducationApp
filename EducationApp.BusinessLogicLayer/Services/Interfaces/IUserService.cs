﻿using EducationApp.BusinessLogicLayer.Models;
using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.BusinessLogicLayer.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Services.Interfaces
{
    public interface IUserService
    {
        public UsersViewModel GetViewModel(UsersViewModel model);
        public Task RegisterAsync(UserModel user);
        public Task<UserModel> UpdateAsync(UserModel user);
        public Task<TokensModel> LoginAsync(UserModel user, bool rememberMe);
        public Task<TokensModel> LoginAsync(LoginViewModel model, bool rememberMe = true);
        public Task<ProfileViewModel> GetProfileViewModelAsync(string username);
        public Task LogoutAsync(string userId);
        public Task LogoutByNameAsync(string userName);
        public Task<TokensModel> RefreshTokenAsync(string accessToken, string refreshToken);
        public Task RemoveAsync(string userId);
        public Task ChangePasswordAsync(UserModel user, string currentPassword, string newPassword);
        public List<UserModel> GetUsers(bool isBlocked, bool isUnblocked, string searchString, int page);
        public int GetPageCount(bool isBlocked, bool isUnblocked, string searchString);
        public Task<UserModel> GetUserByIdAsync(string id);
        public Task<UserModel> GetUserByUsernameAsync(string userName);
        public Task ConfirmEmailAsync(string id, string code);
        public Task ForgotPasswordAsync(string userName);
        public Task ResetPasswordAsync(string userId, string code, string password);

    }
}
