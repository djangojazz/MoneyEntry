using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MoneyEntry.ExpensesAPI.Models;
using MoneyEntry.ExpensesAPI.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyEntry.ExpensesAPI
{
    public abstract class BaseController: Controller
    {
        internal IJWTService JwtService;
        internal JwtSecurityTokenHandler JWTHandler;
        internal IConfiguration Config;

        public BaseController(IJWTService jwt, JwtSecurityTokenHandler jwtHandler, IConfiguration config)
        {
            JwtService = jwt;
            JWTHandler = jwtHandler;
            Config = config;
        }

        internal async Task<UserTokenModel> GetUserModelFromJWT() => await JwtService.GetUserDetailsFromJWTToken(
                ((string)Request.Headers["Authorization"])?.Split(' ')?.Skip(1).FirstOrDefault(),
                JWTHandler,
                Config["Security:Tokens:Key"]);

        internal async Task<JwtSecurityToken> CreateJWT(UserTokenModel request, int personId) =>
            await JwtService.CreateAccessToken(request, personId, Config["Security:Tokens:Key"], Config["Security:Tokens:AccessExpireMinutes"], Config["Security:Tokens:Issuer"], Config["Security:Tokens:Audience"]);
            

        internal async Task<IActionResult> DetermineModelToProceed(Func<Task<IActionResult>> method)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                return await method();
            }
            catch (Exception ex)
            {
                string s = "Fatal Exception";
#if DEBUG
                s = ex.Message;
#endif
                return BadRequest(s);
            }
        }

        internal async Task<IActionResult> CheckPersonToProceed(Func<int, Task<IActionResult>> method)
        {
            var personId = (await GetUserModelFromJWT())?.UserId ?? 0;
            if (personId == 0)
                return BadRequest("There is no user in the header element");

            return await method(personId);
        }
    }
}
