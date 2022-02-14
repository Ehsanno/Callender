using Callender.Model.Authentication;
using Callender.Model.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Callender.Data.TokenGenerator
{
    public class AccessToken
    {
        private readonly AuthenticationConfiguration _configurations;
        private readonly TokenGenerator _tokenGenerator;

        public AccessToken(AuthenticationConfiguration configuration, TokenGenerator tokenGenerator = null)
        {
            _configurations = configuration;
            _tokenGenerator = tokenGenerator;
        }
        public string GenerateToken(User user, Role roleInformation)
        {

            IList<Claim> claims = Array.Empty<Claim>();
            if (claims != null)
            {
                claims = new List<Claim>()
                {
                new Claim(ClaimTypes.SerialNumber, user.ID),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, roleInformation.RoleName)
                };
            }

            return _tokenGenerator.GenerateToken(
                  _configurations.AccessTokenSecret
                , _configurations.Issuer
                , _configurations.Audience
                , _configurations.AccessTokenExpirationMinutes
                , claims
                );
        }
    }
}
