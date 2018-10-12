using MoneyEntry.ExpensesAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace MoneyEntry.ExpensesAPI.Services
{
    public interface IJWTService
    {
        Task<UserTokenModel> GetUserDetailsFromJWTToken(string token, JwtSecurityTokenHandler handler, string key);
        Task<JwtSecurityToken> CreateAccessToken(UserTokenModel request, int personId, string keyInput, string expiresMinutes, string issuerIn, string audienceIn);
    }
}
