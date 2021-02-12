using EducationApp.BusinessLogicLayer.Helpers.Models;
using EducationApp.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Security.Claims;
using System.Text;

namespace EducationApp.BusinessLogicLayer.Helpers.Interfaces
{
    public interface IJwtHelper
    {
        IImmutableDictionary<string, RefreshToken> UsersRefreshTokensReadOnlyDictionary { get; }
        public JwtAuthResult GenerateToken(string userName,List<Claim> claims);
        public void RemoveRefreshTokenByUserName(string userName);
        public JwtAuthResult Refresh(string refreshToken, string accessToken);
    }
}
