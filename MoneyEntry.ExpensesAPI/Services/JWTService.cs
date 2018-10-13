using Microsoft.IdentityModel.Tokens;
using MoneyEntry.ExpensesAPI.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntry.ExpensesAPI.Services
{
    public class JWTService : IJWTService
    {
        public async Task<UserTokenModel> GetUserDetailsFromJWTToken(string token, JwtSecurityTokenHandler handler, string keyInput)
        {
            var validationParameters = new TokenValidationParameters
            {
                RequireExpirationTime = false,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKeys = new[] { new SymmetricSecurityKey(Convert.FromBase64String(keyInput)) }
            };
            handler.ValidateToken(token, validationParameters, out SecurityToken t);
            var claims = ((JwtSecurityToken)t).Claims;

            return await Task.Factory.StartNew(() =>
            {
                return new UserTokenModel
                {
                    UserName = claims.SingleOrDefault(x => x.Type == "unique_name")?.Value ?? string.Empty,
                    UserId = Int32.Parse(claims.SingleOrDefault(x => x.Type == "jti")?.Value ?? "0")
                };
            });
        }
        
        public async Task<JwtSecurityToken> CreateAccessToken(UserTokenModel request, int personId, string keyInput, string expiresMinutes, string issuerIn, string audienceIn)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, request.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, personId.ToString())
            };

            var key = new SymmetricSecurityKey(Convert.FromBase64String(keyInput));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            Int32.TryParse(expiresMinutes, out int minutes);

            if (minutes == 0)
                return null;

            //Just to 
            return await Task.Factory.StartNew(() => new JwtSecurityToken(
                issuer: issuerIn,
                audience: audienceIn,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(minutes),
                signingCredentials: creds));
        }
    }
}
