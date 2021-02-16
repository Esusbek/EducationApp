using EducationApp.BusinessLogicLayer.Models.Helpers;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Security.Claims;

namespace EducationApp.BusinessLogicLayer.Helpers.Interfaces
{
    public interface IJwtProvider
    {
        IImmutableDictionary<string, RefreshToken> UsersRefreshTokensReadOnlyDictionary { get; }
        public JwtAuthResult GenerateToken(string userName, List<Claim> claims);
        public void RemoveRefreshTokenByUserName(string userName);
        public JwtAuthResult Refresh(string refreshToken, string accessToken);
    }
}
