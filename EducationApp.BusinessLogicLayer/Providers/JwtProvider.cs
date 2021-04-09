using EducationApp.BusinessLogicLayer.Models;
using EducationApp.BusinessLogicLayer.Providers.Interfaces;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.Shared.Configs;
using EducationApp.Shared.Constants;
using EducationApp.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Providers
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtConfig _config;
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<UserEntity> _userManager;
        public JwtProvider(IOptions<JwtConfig> config, UserManager<UserEntity> userManager)
        {
            _config = config.Value;
            _userManager = userManager;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key));
        }
        public async Task<TokensModel> GenerateTokenAsync(string userName, string userId, IList<string> userRoles)
        {
            var claims = new List<Claim>
            {
                new Claim(Constants.IDCLAIMNAME, userId)
            };
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(Constants.USERNAMECLAIMNAME, userName));
                claims.Add(new Claim(Constants.ROLECLAIMNAME, role));
            }
            var timeNow = DateTime.UtcNow;
            var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Aud)?.Value);
            var jwtToken = new JwtSecurityToken(
                _config.Issuer,
                shouldAddAudienceClaim ? _config.Audience : string.Empty,
                claims,
                expires: timeNow.AddMinutes(_config.AccessLifetime),
                signingCredentials: new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature));
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var refreshToken = GenerateRefreshTokenString();
            var user = await _userManager.FindByNameAsync(userName);
            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);
            return new TokensModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task RemoveRefreshTokenAsync(UserEntity user)
        {
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
        }

        public async Task<TokensModel> RefreshAsync(string refreshToken, string accessToken)
        {
            var (principal, jwtToken) = DecodeJwtToken(accessToken);
            if (jwtToken is null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.INVALIDTOKENERROR);
            }
            var id = jwtToken.Claims.FirstOrDefault(claim => claim.Type.Equals(Constants.IDCLAIMNAME)).Value;
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                throw new CustomApiException(HttpStatusCode.Unauthorized, Constants.INVALIDTOKENERROR);
            }
            if (user.RefreshToken is null || !user.RefreshToken.Equals(refreshToken))
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.INVALIDTOKENERROR);
            }
            var roles = await _userManager.GetRolesAsync(user);
            return await GenerateTokenAsync(user.UserName, user.Id, roles);
        }

        private (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.INVALIDTOKENERROR);
            }
            var principal = new JwtSecurityTokenHandler()
                .ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = _config.Issuer,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = _key,
                        ValidAudience = _config.Audience,
                        ValidateAudience = true,
                        ValidateLifetime = false,
                        ClockSkew = TimeSpan.FromMinutes(_config.ClockSkew)
                    },
                    out var validatedToken);
            return (principal, validatedToken as JwtSecurityToken);
        }

        private static string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[Constants.DEFAULTREFRESHTOKENLENGTH];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
