using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyEntry.DataAccess.EFCore;

namespace MoneyEntry.ExpensesAPI.Controllers
{
    [Route("expensesApi/[controller]/[action]"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoriesController : BaseController
    {
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
