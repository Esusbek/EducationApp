using EducationApp.BusinessLogicLayer.Models.Helpers;
using EducationApp.DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Providers.Interfaces
{
    public interface IJwtProvider
    {
        public Task<TokenHelperModel> GenerateTokenAsync(string userName, string usedId, IList<string> userRoles);
        public Task RemoveRefreshTokenAsync(UserEntity user);
        public Task<TokenHelperModel> RefreshAsync(string refreshToken, string accessToken);
    }
}
