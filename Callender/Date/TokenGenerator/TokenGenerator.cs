
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Callender.Data.TokenGenerator
{
    public class TokenGenerator
    {

        public string GenerateToken(string SecretKey, string Issuer, string Audience, double ExpirationMinutes, IEnumerable<Claim> claims = null)
        {
            
            SecurityKey key = new SymmetricSecurityKey
                         (Encoding.UTF8.GetBytes(SecretKey));
            SigningCredentials credetials = new 
                (key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
            Issuer
            , Audience
            , claims
            , DateTime.UtcNow
            , DateTime.UtcNow.AddMinutes(ExpirationMinutes)
            , credetials
             );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}