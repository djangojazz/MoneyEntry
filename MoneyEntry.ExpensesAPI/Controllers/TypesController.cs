using System.IdentityModel.Tokens.Jwt;
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
    public class TypesController : BaseController
    {
        ExpensesRepository _repo = ExpensesRepository.Instance;

        public TypesController(IJWTService jwtService, JwtSecurityTokenHandler handler, IConfiguration configuration) : base(jwtService, handler, configuration) { }
        
        [HttpGet]
        public async Task<IActionResult> GetTypes() => Ok(await _repo.GetTypesAsync());
    }
}