using EducationApp.BusinessLogicLayer.Models.Helpers;
using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Services.Interfaces
{
    public interface IUserService
    {
        public Task RegisterAsync(UserModel user);
        public Task<UserModel> UpdateAsync(UserModel user);
        public Task<TokensModel> LoginAsync(UserModel user, bool rememberMe);
        public Task LogoutAsync(string userId);
        public Task<TokensModel> RefreshTokenAsync(string accessToken, string refreshToken);
        public Task BanAsync(UserModel user, int duration=0);
        public Task ChangePasswordAsync(UserModel user, string currentPassword, string newPassword);
        public List<UserModel> GetUsers(int page);
        public Task<UserModel> GetUserByIdAsync(string id);
        public Task<UserModel> GetUserByUsernameAsync(string userName);
        public Task ConfirmEmailAsync(string id, string code);
        public Task ForgotPasswordAsync(string userName);
        public Task ResetPasswordAsync(string userId, string code, string password);

    }
}
