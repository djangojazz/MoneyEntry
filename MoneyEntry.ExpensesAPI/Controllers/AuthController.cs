using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MoneyEntry.DataAccess.EFCore;
using MoneyEntry.ExpensesAPI.Models;

namespace MoneyEntry.ExpensesAPI.Controllers
{
    [Route("expensesApi/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _config;
        ExpensesRepository _repo = ExpensesRepository.Instance;

        public AuthController(IHostingEnvironment env, IConfiguration config)
        {
            _env = env;
            _config = config;
        }

        [HttpGet, Route("{userName}")]
        public async Task<IActionResult> GetSalt(string userName)
        {
            if (userName == null)
                return BadRequest("Need a User Name");

            var user = await _repo.GetPersonAsync(userName);
            if (user == null)
                return NotFound("User does not exist");

            return Ok(user.Salt);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateUserToken([FromBody] UserTokenModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (request.Password == null)
                return BadRequest("Password is incorrect or missing");

            try
            {
                //await _repo.UpdatePasswordAsync(request.UserName, request.Password);

                var user = await _repo.GetPersonAsync(request.UserName);

                if (Convert.ToBase64String(user.Password) != Convert.ToBase64String(request.Password))
                    return BadRequest("Password is incorrect");

                // generate access and refresh tokens
                var tokenPair = await CreateAccessToken(request);

                var handler = new JwtSecurityTokenHandler();
                var accessJwt = handler.WriteToken(tokenPair);

                return Ok(new { token = accessJwt });

            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                {
                    return BadRequest(new
                    {
                        Msg = "Exception in token request",
                        Env = _env.EnvironmentName,
                        Time = DateTime.UtcNow.ToString(),
                        Exception = ex
                    });
                }
            }

            return BadRequest();
        }
        

        private async Task<JwtSecurityToken> CreateAccessToken(UserTokenModel request)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, request.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Convert.FromBase64String(_config["Security:Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            Int32.TryParse(_config["Security:Tokens:AccessExpireMinutes"], out int minutes);

            if (minutes == 0)
                return null;
            
            //Just to 
            return await Task.Factory.StartNew(() => new JwtSecurityToken(
                issuer: _config["Security:Tokens:Issuer"],
                audience: _config["Security:Tokens:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(minutes),
                signingCredentials: creds));
        }
    }
}