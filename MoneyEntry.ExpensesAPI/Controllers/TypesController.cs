using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoneyEntry.DataAccess.EFCore;

namespace MoneyEntry.ExpensesAPI.Controllers
{
    [Route("api/[controller]")]
    public class TypesController : Controller
    {
        ExpensesRepository _repo = ExpensesRepository.Instance;
        
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _repo.GetTypesAsync());
    }
}
