using EducationApp.BusinessLogicLayer.Models;
using EducationApp.BusinessLogicLayer.Models.Requests;
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
        public Task<TokensModel> GoogleLoginAsync(string idToken);
        public Task LogoutAsync(string userId);
        public Task LogoutByNameAsync(string userName);
        public Task<TokensModel> RefreshTokenAsync(string accessToken, string refreshToken);
        public Task BanAsync(string userId);
        public Task ChangePasswordAsync(UserModel user, string currentPassword, string newPassword);
        public List<UserModel> GetUsers(bool getBlocked, bool getUnblocked, string searchString, int page);
        public int GetLastPage(bool getBlocked, bool getUnblocked, string searchString);
        public Task<UserModel> GetUserByIdAsync(string id);
        public Task<UserModel> GetUserByUsernameAsync(string userName);
        public Task ConfirmEmailAsync(string id, string code);
        public Task ForgotPasswordAsync(string userName);
        public Task ResetPasswordAsync(string userId, string code, string password);

    }
}
