using EducationApp.BusinessLogicLayer.Models.Users;
using EducationApp.DataAccessLayer.Entities;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Services.Interfaces
{
    public interface IUserService
    {
        public Task RegisterAsync(UserModel user);
        public void UpdateAsync(UserModel user);
        public Task<UserModel> LoginAsync(UserModel user, bool rememberMe);
        public Task<bool> AuthorizationAsync(UserEntity user, string role);
        public void BanAsync(UserModel user, int duration);
        public UserModel GetUsers();
        public Task<UserEntity> GetUserAsync(string id);
        public void AddRoleAsync(UserEntity user, string role);
        public Task ConfirmEmailAsync(string id, string code);

    }
}
