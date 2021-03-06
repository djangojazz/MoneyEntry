﻿using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MoneyEntry.DataAccess.EFCore;
using MoneyEntry.ExpensesAPI.Services;

namespace MoneyEntry.ExpensesAPI.Controllers
{
    [Route("expensesApi/[controller]/[action]"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoriesController : BaseController
    {
        public CategoriesController(IJWTService jwt, JwtSecurityTokenHandler handler, IConfiguration configuration) : base(jwt, handler, configuration) {}

        ExpensesRepository _repo = ExpensesRepository.Instance;

        [HttpGet]
        public async Task<IActionResult> GetCategories() => Ok((await _repo.GetCategoriesAsync()).ToList());
        
        
        // POST: api/Categories
        [HttpPost]
        public async Task<IActionResult> PostCategory([FromBody]string value)
        {
            await _repo.AddCategoryAsync(value);
            
            var results = (await _repo.GetCategoriesAsync()).ToList();

            if (results.Exists(x => x.Description == value))
            { 
                return Ok(results);
            }
            else
            {
                return BadRequest($"Could not create category {value}");
            }
        }
    }
}
