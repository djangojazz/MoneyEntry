using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MoneyEntry.DataAccess.EFCore;
using MoneyEntry.ExpensesAPI.Models;
using MoneyEntry.ExpensesAPI.Services;

namespace MoneyEntry.ExpensesAPI.Controllers
{
    [Route("expensesApi/[controller]/[action]"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TransactionsController : BaseController
        //Controller
    {
        ExpensesRepository _repo = ExpensesRepository.Instance;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TransactionsController(IJWTService jwt, JwtSecurityTokenHandler handler, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(jwt, handler, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetLastDate()
        {
            //var token2 = await _httpContextAccessor.HttpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, "access_token");
            var person = await GetUserModelFromJWT();
            return Ok(await _repo.LastDateEnteredByPersonAsync(person.UserId));
        }

        [HttpGet]
        public async Task<IActionResult> GetDate() => Ok(await Task.Factory.StartNew(() => new DateTime(2018, 2, 1)));

        //Optional params as needed
        [HttpGet, Route("{personId?}/{start?}/{end?}")]
        public async Task<IActionResult> GetTransactions(int personId = 1, DateTime? start = null, DateTime? end = null)
        {
            var trans = await _repo.GetTransactionViewsAsync(start ?? DateTime.Now.Date.AddMonths(-3), end ?? DateTime.Now, personId);

            if (!trans.Any())
               return NotFound("There were no results found for this time period");

            return Ok(trans);
        }
        
        // POST: api/Transactions
        [HttpPost]
        public async Task<IActionResult> PostTransaction([FromBody]TransactionModel t) =>
         await DetermineModelThenReturn(async () => Ok(await _repo.InsertOrUpdaTeTransactionAsync(t.TransactionId, t.Amount, t.Description, t.TypeId, t.CategoryId, t.CreatedDate, t.PersonId, t.Reconciled)));   
    }
}
