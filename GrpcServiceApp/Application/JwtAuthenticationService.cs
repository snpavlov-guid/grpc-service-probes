using GrpcServiceApp.Common;
using GrpcServiceApp.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GrpcServiceApp.Application
{
    public class JwtAuthenticationService : IAuthenticationService
    {
        private readonly JwtBearerSettings _settings;

        public JwtAuthenticationService(IConfiguration configuration)
        {
            _settings = configuration.GetSection(Defaults.JwtBearerEntry).Get<JwtBearerSettings>();
        }

        #region IAuthenticationService

        public string Authenticaticate(string username, string password)
        {
            // Any non empty username and password are allowed
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new AuthenticationException("Any non empty username and password are allowed");
            }

            return GetToken(username);
        }

        #endregion

        #region Implementation

        public string GetToken(string username)
        {
            // If user is not authenticated return token without claims

            var creds = GetSigningCredentialsFromBase64String(_settings.JwtKey);

            var expires = DateTime.Now.AddMinutes(_settings.JwtExpireMinutes);

            var token = new JwtSecurityToken(
                issuer: _settings.JwtIssuer,
                audience: _settings.JwtAudience,
                expires: expires,
                signingCredentials: creds,
                claims: GetUserClaims(username)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        protected List<Claim> GetUserClaims(string username)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
            };

            return claims;
        }

        protected SigningCredentials GetSigningCredentialsFromBase64String(string strkey, string algorithm = SecurityAlgorithms.HmacSha256)
        {
            return new SigningCredentials(Helpers.GetSecurityKeyFromBase64String(strkey), algorithm);
        }

        #endregion
    }
}
