using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MoneyEntry.DataAccess.EFCore;
using MoneyEntry.ExpensesAPI.Models;
using MoneyEntry.ExpensesAPI.Services;

namespace MoneyEntry.ExpensesAPI.Controllers
{
    [Route("expensesApi/[controller]/[action]")]
    public class AuthController : BaseController
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _config;
        ExpensesRepository _repo = ExpensesRepository.Instance;

        public AuthController(IHostingEnvironment env, IConfiguration config, IJWTService jwtService) : base(jwtService)
        {
            _env = env;
            _config = config;
        }

        [HttpGet, Route("{userName}")]
        public async Task<IActionResult> GetSalt(string userName)
        {
            try
            {
                if (userName == null)
                    return BadRequest("Need a User Name");
                
                var user = await _repo.GetPersonAsync(userName);
                if (user == null)
                    return NotFound("User does not exist");

                return Ok(user.Salt);
            }
            catch (Exception)
            {
                return BadRequest("Could not retrieve the salt");
            }
        }

        [HttpPost()]
        public async Task<IActionResult> GenerateSalt([FromBody] UserTokenModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (request.Salt == null)
                return BadRequest("Salt is incorrect or missing");

            if (!(await _repo.PersonExistsAsync(request.UserName)))
                return BadRequest("User does not exist");
            
            try
            {
                await _repo.GenerateSaltAsync(request.UserName, request.Salt);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Could not update the salt");
            }
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
                if (user == null)
                    return NotFound("User does not exist");

                if (Convert.ToBase64String(user.Password) != Convert.ToBase64String(request.Password))
                    return BadRequest("Password is incorrect");

                var tokenPair = await JwtService.CreateAccessToken(request, user.PersonId, _config["Security:Tokens:Key"], _config["Security:Tokens:AccessExpireMinutes"], _config["Security:Tokens:Issuer"], _config["Security:Tokens:Audience"]);
                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(tokenPair) });

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
        

        private async Task<JwtSecurityToken> CreateAccessToken(UserTokenModel request, int personId)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, request.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, personId.ToString())
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