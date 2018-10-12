using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyEntry.DataAccess.EFCore;
using MoneyEntry.ExpensesAPI.Services;

namespace MoneyEntry.ExpensesAPI.Controllers
{
    [Route("expensesApi/[controller]/[action]"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TypesController : BaseController
    {
        ExpensesRepository _repo = ExpensesRepository.Instance;

        public TypesController(IJWTService jwtService) : base(jwtService) { }
        
        [HttpGet]
        public async Task<IActionResult> GetTypes() => Ok(await _repo.GetTypesAsync());
    }
}