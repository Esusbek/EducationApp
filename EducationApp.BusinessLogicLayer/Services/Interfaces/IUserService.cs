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
        public Task UpdateAsync(UserModel user);
        public Task<TokenHelperModel> LoginAsync(UserModel user, bool rememberMe);
        public Task LogoutAsync(string userId);
        public Task<TokenHelperModel> RefreshTokenAsync(string accessToken, string refreshToken);
        public Task<bool> AuthorizationAsync(UserEntity user, string role);
        public void BanAsync(UserModel user, int duration);
        public List<UserModel> GetUsers();
        public Task<UserEntity> GetUserByIdAsync(string id);
        public Task<UserEntity> GetUserByUsernameAsync(string userName);
        public Task ConfirmEmailAsync(string id, string code);
        public Task ForgotPasswordAsync(string userName);
        public Task ResetPasswordAsync(string userId, string code, string password);

    }
}
