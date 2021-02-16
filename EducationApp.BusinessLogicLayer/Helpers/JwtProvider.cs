using EducationApp.BusinessLogicLayer.Helpers.Interfaces;
using EducationApp.BusinessLogicLayer.Models.Helpers;
using EducationApp.Shared.Configs;
using EducationApp.Shared.Constants;
using EducationApp.Shared.Exceptions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EducationApp.BusinessLogicLayer.Helpers
{
    public class JwtProvider : IJwtProvider
    {
        public IImmutableDictionary<string, RefreshToken> UsersRefreshTokensReadOnlyDictionary => _usersRefreshTokens.ToImmutableDictionary();
        private readonly ConcurrentDictionary<string, RefreshToken> _usersRefreshTokens;
        private readonly JwtConfig _config;
        private readonly SymmetricSecurityKey _key;

        public JwtProvider(IOptions<JwtConfig> config)
        {
            _config = config.Value;
            _usersRefreshTokens = new ConcurrentDictionary<string, RefreshToken>();
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key));
        }

        public JwtAuthResult GenerateToken(string userName, List<Claim> claims)
        {
            var timeNow = DateTime.Now;
            var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);
            var jwtToken = new JwtSecurityToken(
                _config.Issuer,
                shouldAddAudienceClaim ? _config.Audience : string.Empty,
                claims,
                expires: timeNow.AddMinutes(_config.AccessLifetime),
                signingCredentials: new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature));
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var refreshToken = new RefreshToken
            {
                UserName = userName,
                TokenString = GenerateRefreshTokenString(),
                ExpireAt = timeNow.AddMinutes(_config.RefreshLifetime)
            };

            _usersRefreshTokens.AddOrUpdate(refreshToken.TokenString, refreshToken, (s, t) => refreshToken);

            return new JwtAuthResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public void RemoveRefreshTokenByUserName(string userName)
        {
            var refreshTokens = _usersRefreshTokens.Where(x => x.Value.UserName == userName).ToList();
            foreach (var refreshToken in refreshTokens)
            {
                _usersRefreshTokens.TryRemove(refreshToken.Key, out _);
            }
        }

        public JwtAuthResult Refresh(string refreshToken, string accessToken)
        {
            var (principal, jwtToken) = DecodeJwtToken(accessToken);
            if (jwtToken is null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.Errors.InvalidToken);
            }

            var userName = principal.Identity.Name;
            if (!_usersRefreshTokens.TryGetValue(refreshToken, out var existingRefreshToken))
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.Errors.InvalidToken);
            }
            if (existingRefreshToken.UserName != userName || existingRefreshToken.ExpireAt < DateTime.Now)
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.Errors.InvalidToken);
            }

            return GenerateToken(userName, principal.Claims.ToList());
        }

        public (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new CustomApiException(HttpStatusCode.UnprocessableEntity, Constants.Errors.InvalidToken);
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
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(_config.ClockSkew)
                    },
                    out var validatedToken);
            return (principal, validatedToken as JwtSecurityToken);
        }

        private static string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
